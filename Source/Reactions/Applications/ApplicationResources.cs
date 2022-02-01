// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Aksio.Cratis.Events.Projections.Definitions;
using Aksio.Cratis.Events.Projections.Grains;
using Aksio.Cratis.Json;
using Aksio.Cratis.Schemas;
using Events.Applications;
using Events.Organizations;
using Microsoft.Extensions.Logging;
using Orleans;
using Pulumi.Automation;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;

namespace Reactions.Applications
{
    [Observer("c2f0e081-6a1a-46e0-bc8c-8a08e0c4dff5")]
    public class ApplicationResources
    {
        static readonly JsonSerializerOptions _serializerOptions;
        readonly ILogger<ApplicationResources> _logger;
        readonly IClusterClient _clusterClient;
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
            IJsonSchemaGenerator schemaGenerator)
        {
            _logger = logger;
            _clusterClient = clusterClient;
            var projectionBuilder = new ProjectionBuilderFor<Application>("c2f0e081-6a1a-46e0-bc8c-8a08e0c4dff5", eventTypes, schemaGenerator);
            _projectionDefinition = projectionBuilder
                .WithName($"Reaction: {nameof(ApplicationResources)} - {nameof(Application)}")
                .Passive()
                .NotRewindable()
                .From<ApplicationCreated>(_ => _
                    .Set(m => m.Name).To(e => e.Name)
                    .Set(m => m.AzureSubscriptionId).To(e => e.AzureSubscriptionId)
                    .Set(m => m.CloudLocation).To(e => e.CloudLocation))
                .From<PulumiAccessTokenSet>(_ => _
                    .Set(m => m.PulumiAccessToken).To(e => e.AccessToken))
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
            var application = await GetApplication(context.EventSourceId);
            await RunOrDestroyProgram(application, false);
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
                var subscription = Environment.GetEnvironmentVariable("AZURE_SUBSCRIPTION")!;
                var resourceGroup = ResourceGroup.Get("EinarDev", $"/subscriptions/{subscription}/resourceGroups/EinarDev");
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
            });

            var args = new InlineProgramArgs("my-first-project", "dev", program);
            var stack = await LocalWorkspace.CreateOrSelectStackAsync(args);
            await stack.Workspace.InstallPluginAsync("azure-native", "1.54.0");
            await stack.SetAllConfigAsync(new Dictionary<string, ConfigValue>
            {
                { "azure-native:location", new ConfigValue(application.CloudLocation) },
                { "azure-native:subscriptionId", new ConfigValue(application.AzureSubscriptionId.ToString()) }
            });

            Environment.SetEnvironmentVariable("PULUMI_ACCESS_TOKEN", application.PulumiAccessToken);

            if (!isDestroy)
            {
                await stack.UpAsync(new UpOptions { OnStandardOutput = Console.WriteLine });
            }
            else
            {
                await stack.DestroyAsync(new DestroyOptions { OnStandardOutput = Console.WriteLine });
            }
        }
    }
}
