// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Events.Applications.Environments.Microservices.Deployables;
using Events.Applications.Microservices.Deployables;
using Microsoft.Extensions.Logging;

namespace Reactions.Applications;

[Observer("af7c5f3f-5099-4ebe-bf5b-307e7061c178")]
public class ApplicationDeploymentCoordinator
{
    readonly IImmediateProjections _projections;
    readonly IEventLog _eventLog;
    readonly ILogger<ApplicationDeploymentCoordinator> _logger;

    public ApplicationDeploymentCoordinator(
        IImmediateProjections projections,
        IEventLog eventLog,
        ILogger<ApplicationDeploymentCoordinator> logger)
    {
        _projections = projections;
        _eventLog = eventLog;
        _logger = logger;
    }

    public async Task DeployableImageChanged(DeployableImageChanged @event, EventContext context)
    {
        var application = await _projections.GetInstanceById<Application>(context.EventSourceId);
        var applicationEnvironments = await _projections.GetInstanceById<ApplicationEnvironmentsForApplication>(context.EventSourceId);

        foreach (var environment in applicationEnvironments.Model.Environments)
        {
            await _eventLog.Append(environment.Id, new DeployableImageChangedInEnvironment(
                context.EventSourceId,
                environment.Id,
                @event.MicroserviceId,
                ApplicationEnvironmentDeploymentId.New(),
                @event.DeployableId,
                @event.Image));
        }

        _logger.ChangingDeployableImage(@event.DeployableId, @event.MicroserviceId, @event.Image, application.Model.Name);
    }
}
