// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Aksio.Cratis.Events.Projections.Definitions;
using Aksio.Cratis.Events.Projections.Grains;
using Aksio.Cratis.Json;
using Aksio.Cratis.Schemas;
using Events.Applications;
using Microsoft.Extensions.Logging;
using Orleans;

namespace Reactions.Applications
{
    [Observer("c2f0e081-6a1a-46e0-bc8c-8a08e0c4dff5")]
    public class ApplicationResources
    {
        static readonly JsonSerializerOptions _serializerOptions;
        readonly ILogger<ApplicationResources> _logger;
        readonly IClusterClient _clusterClient;
        readonly IPulumiStackDefinitions _stackDefinitions;
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
            IPulumiStackDefinitions stackDefinitions,
            IPulumiRunner pulumiRunner)
        {
            _logger = logger;
            _clusterClient = clusterClient;
            _stackDefinitions = stackDefinitions;
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
            var application = await GetApplication(context.EventSourceId);
            Console.WriteLine(application);

            // var definition = _stackDefinitions.CreateApplication(application, RuntimeEnvironment.Development);
            // await _pulumiRunner.Up(application, application.Name, definition, RuntimeEnvironment.Development);
            await Task.CompletedTask;
        }

        public async Task Removed(ApplicationRemoved @event, EventContext context)
        {
            // var application = await GetApplication(context.EventSourceId);
            // var definition = _stackDefinitions.CreateApplication(application, RuntimeEnvironment.Development);
            // await _pulumiRunner.Down(application, application.Name, definition, RuntimeEnvironment.Development);
            await Task.CompletedTask;
        }

        async Task<Application> GetApplication(EventSourceId eventSourceId)
        {
            var projection = _clusterClient.GetGrain<IProjection>(_projectionDefinition.Identifier);
            var json = await projection.GetModelInstanceById(eventSourceId);
            json["id"] = eventSourceId.Value;
            return json.Deserialize<Application>(_serializerOptions)!;
        }
    }
}
