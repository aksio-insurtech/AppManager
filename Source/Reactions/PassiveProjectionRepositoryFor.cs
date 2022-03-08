// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Aksio.Cratis.Events.Projections.Definitions;
using Aksio.Cratis.Events.Projections.Grains;
using Aksio.Cratis.Execution;
using Aksio.Cratis.Json;
using Aksio.Cratis.Schemas;
using Aksio.Cratis.Types;
using Orleans;

namespace Reactions;

[Singleton]
public class PassiveProjectionRepositoryFor<TModel> : IPassiveProjectionRepositoryFor<TModel>
{
    static readonly JsonSerializerOptions _serializerOptions;
    readonly ProjectionDefinition? _projectionDefinition;
    readonly IClusterClient _clusterClient;

    static PassiveProjectionRepositoryFor()
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

    public PassiveProjectionRepositoryFor(
        IInstancesOf<IPassiveProjectionFor<TModel>> projectionDefinitions,
        IClusterClient clusterClient,
        IEventTypes eventTypes,
        IJsonSchemaGenerator schemaGenerator)
    {
        _clusterClient = clusterClient;
        var definer = projectionDefinitions.SingleOrDefault();
        if (definer is not null)
        {
            var builder = new ProjectionBuilderFor<TModel>(definer.Identifier, eventTypes, schemaGenerator)
                .WithName($"PassiveProjection: {typeof(TModel).FullName}")
                .Passive()
                .NotRewindable();

            definer.Define(builder);
            _projectionDefinition = builder.Build();

            var projections = _clusterClient.GetGrain<IProjections>(Guid.Empty);
            var pipelineDefinition = new ProjectionPipelineDefinition(
                _projectionDefinition.Identifier,
                "c0c0196f-57e3-4860-9e3b-9823cf45df30", // Cratis default
                Array.Empty<ProjectionResultStoreDefinition>());

            projections.Register(_projectionDefinition, pipelineDefinition);
        }
    }

    public async Task<TModel> GetById(EventSourceId eventSourceId)
    {
        if (_projectionDefinition is null)
        {
            return default!;
        }
        var projection = _clusterClient.GetGrain<IProjection>(_projectionDefinition.Identifier);
        var json = await projection.GetModelInstanceById(eventSourceId);
        json["id"] = eventSourceId.Value;
        return json.Deserialize<TModel>(_serializerOptions)!;
    }
}
