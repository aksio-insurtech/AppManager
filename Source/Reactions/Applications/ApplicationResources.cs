// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Aksio.Cratis.Events.Projections.Definitions;
using Aksio.Cratis.Events.Projections.Grains;
using Aksio.Cratis.Json;
using Aksio.Cratis.Schemas;
using Common;
using Events.Applications;
using Events.Organizations;
using Microsoft.Extensions.Logging;
using Orleans;
using Pulumi.Automation;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using Pulumi.Mongodbatlas;

namespace Reactions.Applications
{
    [Observer("c2f0e081-6a1a-46e0-bc8c-8a08e0c4dff5")]
    public class ApplicationResources
    {
        static readonly JsonSerializerOptions _serializerOptions;
        readonly ILogger<ApplicationResources> _logger;
        readonly IClusterClient _clusterClient;
        readonly IPulumiRunner _pulumiRunner;
        readonly ProjectionDefinition _projectionDefinition;

        static ApplicationResources()
        {
            _serializerOptions = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters =
                {
                    new ConceptAsJsonConverterFactory()
                }
            };
        }

        public ApplicationResources(
            ILogger<ApplicationResources> logger,
            IClusterClient clusterClient,
            IEventTypes eventTypes,
            IJsonSchemaGenerator schemaGenerator,
            IPulumiRunner pulumiRunner)
        {
            _logger = logger;
            _clusterClient = clusterClient;
            _pulumiRunner = pulumiRunner;
            var projectionBuilder = new ProjectionBuilderFor<Application>("c2f0e081-6a1a-46e0-bc8c-8a08e0c4dff5", eventTypes, schemaGenerator);
            _projectionDefinition = projectionBuilder
                .WithName($"Reaction: {nameof(ApplicationResources)} - {nameof(Application)}")
                .Passive()
                .NotRewindable()
                .From<ApplicationCreated>(_ => _
                    .Set(m => m.Name).To(e => e.Name)
                    .Set(m => m.AzureSubscriptionId).To(e => e.AzureSubscriptionId)
                    .Set(m => m.CloudLocation).To(e => e.CloudLocation))
                .Build();

            var projections = _clusterClient.GetGrain<IProjections>(Guid.Empty);
            var pipelineDefinition = new ProjectionPipelineDefinition(
                _projectionDefinition.Identifier,
                "c0c0196f-57e3-4860-9e3b-9823cf45df30", // Cratis default
                Array.Empty<ProjectionResultStoreDefinition>());

            projections.Register(_projectionDefinition, pipelineDefinition);
        }

        public async Task Created(ApplicationCreated @event, EventContext context)
        {
            // var application = await GetApplication(context.EventSourceId);
            // await RunOrDestroyProgram(application, false);
            await Task.CompletedTask;
        }

        public async Task Removed(ApplicationRemoved @event, EventContext context)
        {
            var application = await GetApplication(context.EventSourceId);
            await RunOrDestroyProgram(application, true);
        }

        async Task<Application> GetApplication(EventSourceId eventSourceId)
        {
            var projection = _clusterClient.GetGrain<IProjection>(_projectionDefinition.Identifier);
            var json = await projection.GetModelInstanceById(eventSourceId);
            json["id"] = eventSourceId.Value;
            return json.Deserialize<Application>(_serializerOptions)!;
        }

        async Task RunOrDestroyProgram(Application application, bool isDestroy = false)
        {
            var program = PulumiFn.Create(() =>
            {
                var resourceGroup = ResourceGroup.Get("EinarDev", $"/subscriptions/{application.AzureSubscriptionId}/resourceGroups/EinarDev");
                var storageAccount = new StorageAccount(application.Name.Value.ToLowerInvariant(), new StorageAccountArgs
                {
                    ResourceGroupName = resourceGroup.Name,
                    Tags = new Dictionary<string, string>
                    {
                        { "application", application.Id.ToString() }
                    },
                    Sku = new SkuArgs
                    {
                        Name = SkuName.Standard_LRS
                    },
                    Kind = Kind.StorageV2
                });

                var project = new Project(application.Name, new ProjectArgs
                {
                    OrgId = "61f150d98bc1f86a0748984d"
                });

                var cluster = new Cluster("dev", new ClusterArgs
                {
                    ProjectId = project.Id,
                    ProviderName = "TENANT",
                    BackingProviderName = "AZURE",
                    ProviderInstanceSizeName = "M0",
                    ProviderRegionName = "EUROPE_NORTH"
                });
            });

            // Notes:
            // - Organization settings: Atlas OrgId
            // - Create MongoDB cluster for application
            // - Create MongoDB database user
            // - Automatically create dev & prod stacks
            // - Setup Kernel container instance
            // - Add tag for application for each stack
            // - Add tag for environment for each stack
            // - Store needed output values - show on resources tab
            // - Output
            var args = new InlineProgramArgs(application.Name, "dev", program)
            {
                ProjectSettings = new(application.Name, ProjectRuntimeName.Dotnet)
            };
            var stack = await LocalWorkspace.CreateOrSelectStackAsync(args);
            await stack.Workspace.InstallPluginAsync("azure-native", "1.54.0");
            await stack.Workspace.InstallPluginAsync("mongodbatlas", "3.2.0");
            await stack.SetAllConfigAsync(new Dictionary<string, ConfigValue>
            {
                { "azure-native:location", new ConfigValue(application.CloudLocation) },
                { "azure-native:subscriptionId", new ConfigValue(application.AzureSubscriptionId.ToString()) }
            });

            Environment.SetEnvironmentVariable("MONGODB_ATLAS_PUBLIC_KEY", "mhzsqyko");
            Environment.SetEnvironmentVariable("MONGODB_ATLAS_PRIVATE_KEY", "dcc8e479-e163-4aff-9632-564d5d213cef");

            if (!isDestroy)
            {
                _pulumiRunner.Up(stack);
            }
            else
            {
                _pulumiRunner.Down(stack);
            }
        }
    }
}
