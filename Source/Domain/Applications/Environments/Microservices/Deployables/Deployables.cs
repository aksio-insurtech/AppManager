// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Events.Applications.Environments.Microservices.Deployables;

namespace Domain.Applications.Environments.Microservices.Deployables;

[Route("/api/applications/{applicationId}/environments/{environmentId}/microservices/{microserviceId}/deployables")]
public class Deployables : Controller
{
    readonly IEventLog _eventLog;

    public Deployables(IEventLog eventLog) => _eventLog = eventLog;

    [HttpPost]
    public Task CreateDeployable(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromBody] CreateDeployable command) => _eventLog.Append(command.DeployableId.ToString(), new DeployableCreated(applicationId, environmentId, microserviceId, command.Name));

    [HttpPost("with-image")]
    public async Task CreateDeployableWithImage(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromBody] CreateDeployableWithImage command)
    {
        await _eventLog.Append(command.DeployableId.ToString(), new DeployableCreated(applicationId, environmentId, microserviceId, command.Name));
        await _eventLog.Append(command.DeployableId.ToString(), new DeployableImageChanged(command.Image));
    }
}
