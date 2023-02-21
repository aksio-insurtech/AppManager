// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Events.Applications.Environments;
using Events.Applications.Environments.Microservices.Deployables;
using Microsoft.Extensions.Logging;
using Reactions.Applications.Pulumi;

namespace Reactions.Applications;

#pragma warning disable IDE0052

[Observer("c2f0e081-6a1a-46e0-bc8c-8a08e0c4dff5")]
public class ApplicationResourcesCoordinator
{
    readonly ILogger<ApplicationResources> _logger;
    readonly IImmediateProjections _projections;
    readonly ExecutionContext _executionContext;
    readonly IExecutionContextManager _executionContextManager;
    readonly IPulumiStackDefinitions _stackDefinitions;
    readonly IPulumiOperations _pulumiOperations;
    readonly IEventLog _eventLog;

    public ApplicationResourcesCoordinator(
        ILogger<ApplicationResources> logger,
        IImmediateProjections projections,
        ExecutionContext executionContext,
        IExecutionContextManager executionContextManager,
        IPulumiStackDefinitions stackDefinitions,
        IPulumiOperations pulumiOperations,
        IEventLog eventLog)
    {
        _logger = logger;
        _projections = projections;
        _executionContext = executionContext;
        _executionContextManager = executionContextManager;
        _stackDefinitions = stackDefinitions;
        _pulumiOperations = pulumiOperations;
        _eventLog = eventLog;
    }

    public Task ConsolidationStarted(ApplicationEnvironmentConsolidationStarted @event, EventContext context)
    {
        _ = Task.Run(async () =>
        {
            var application = await _projections.GetInstanceById<Application>(@event.ApplicationId);
            var environment = await _projections.GetInstanceById<ApplicationEnvironmentWithArtifacts>(context.EventSourceId);

            _logger.ConsolidationStarted(environment.Model.Name, application.Model.Name);
            try
            {
                await _pulumiOperations.ConsolidateEnvironment(application.Model, environment.Model, @event.ConsolidationId);

                // Todo: Set to actual execution context - might not be the right place for this!
                _executionContextManager.Set(_executionContext);
                await _eventLog.Append(context.EventSourceId, new ApplicationEnvironmentConsolidationCompleted(@event.ApplicationId, @event.EnvironmentId, @event.ConsolidationId));
            }
            catch (Exception ex)
            {
                // Todo: Set to actual execution context - might not be the right place for this!
                _executionContextManager.Set(_executionContext);
                var messages = ex.GetAllMessages();
                await _eventLog.Append(context.EventSourceId, new ApplicationEnvironmentConsolidationFailed(@event.ApplicationId, @event.EnvironmentId, @event.ConsolidationId, messages, ex.StackTrace ?? string.Empty));
            }
        });

        return Task.CompletedTask;
    }

    public Task DeployableImageChanged(DeployableImageChanged @event, EventContext context)
    {
        _ = Task.Run(async () =>
        {
            var application = await _projections.GetInstanceById<Application>(@event.ApplicationId);
            var environment = await _projections.GetInstanceById<ApplicationEnvironmentWithArtifacts>(@event.EnvironmentId);
            var microservice = environment.Model.GetMicroserviceById(@event.MicroserviceId);
            var deployable = microservice.GetDeployableById(@event.DeployableId);

            _logger.ChangingDeployableImage(
                microservice.Name,
                deployable.Name,
                @event.Image,
                environment.Model.Name,
                application.Model.Name);

            try
            {
                await _pulumiOperations.SetImageForDeployable(
                    application.Model,
                    environment.Model,
                    microservice,
                    @event.ConsolidationId,
                    deployable,
                    @event.Image);

                _executionContextManager.Set(_executionContext);
                await _eventLog.Append(context.EventSourceId, new ApplicationEnvironmentConsolidationCompleted(@event.ApplicationId, @event.EnvironmentId, @event.ConsolidationId));
            }
            catch (Exception ex)
            {
                // Todo: Set to actual execution context - might not be the right place for this!
                _executionContextManager.Set(_executionContext);
                var messages = ex.GetAllMessages();
                await _eventLog.Append(context.EventSourceId, new ApplicationEnvironmentConsolidationFailed(@event.ApplicationId, @event.EnvironmentId, @event.ConsolidationId, messages, ex.StackTrace ?? string.Empty));
            }
        });

        return Task.CompletedTask;
    }
}
