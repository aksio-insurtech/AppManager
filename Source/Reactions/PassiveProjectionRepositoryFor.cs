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
    readonly IExecutionContextManager _executionContextManager;

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
        IJsonSchemaGenerator schemaGenerator,
        IExecutionContextManager executionContextManager)
    {
        _clusterClient = clusterClient;
        _executionContextManager = executionContextManager;
        var definer = projectionDefinitions.SingleOrDefault();
        if (definer is not null)
        {
            var builder = new ProjectionBuilderFor<TModel>(definer.Identifier, eventTypes, schemaGenerator)
                .WithName($"PassiveProjection: {typeof(TModel).FullName}")
                .NotRewindable();

            definer.Define(builder);
            _projectionDefinition = builder.Build();

            var projections = _clusterClient.GetGrain<IProjections>(Guid.Empty);
            var pipelineDefinition = new ProjectionPipelineDefinition(
                _projectionDefinition.Identifier,
                Array.Empty<ProjectionSinkDefinition>());

            projections.Register(_projectionDefinition, pipelineDefinition);
        }
    }

    public async Task<TModel> GetById(EventSourceId eventSourceId)
    {
        if (_projectionDefinition is null)
        {
            return default!;
        }

        var executionContext = _executionContextManager.Current;
        var projectionKey = new ProjectionKey(executionContext.MicroserviceId, executionContext.TenantId, EventSequenceId.Log);
        var projection = _clusterClient.GetGrain<IProjection>(_projectionDefinition.Identifier, projectionKey);
        var json = await projection.GetModelInstanceById(eventSourceId);
        json["id"] = eventSourceId.Value;
        return json.Deserialize<TModel>(_serializerOptions)!;
    }
}
