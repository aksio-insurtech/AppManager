// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Events.Applications.Environments.Microservices.Modules.Deployables;

namespace Domain.Applications.Environments.Microservices.Modules.Deployables;

[Route("/api/applications/{applicationId}/environments/{environmentId}/microservices/{microserviceId}/modules/{moduleId}/deployables")]
public class Deployables : Controller
{
    readonly IEventLog _eventLog;

    public Deployables(IEventLog eventLog) => _eventLog = eventLog;

    [HttpPost]
    public Task CreateDeployable(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] ModuleId moduleId,
        [FromBody] CreateDeployable command) => _eventLog.Append(environmentId, new DeployableCreated(environmentId, microserviceId, moduleId, command.DeployableId, command.Name));

    [HttpPost("with-image")]
    public async Task CreateDeployableWithImage(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] ModuleId moduleId,
        [FromBody] CreateDeployableWithImage command)
    {
        await _eventLog.Append(environmentId, new DeployableCreated(environmentId, microserviceId, moduleId, command.DeployableId, command.Name));
        await _eventLog.Append(environmentId, new DeployableImageChangedInEnvironment(applicationId, environmentId, microserviceId, moduleId, ApplicationEnvironmentDeploymentId.New(), command.DeployableId, command.Image));
    }
}
