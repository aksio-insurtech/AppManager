// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Events.Applications.Environments.Microservices.Deployables;

namespace Domain.Applications.Environments.Microservices.Deployables;

[Route("/api/applications/{applicationId}/environments/{environmentId}/microservices/{microserviceId}/deployables/{deployableId}")]
public class Deployable : Controller
{
    readonly IEventLog _eventLog;

    public Deployable(IEventLog eventLog) => _eventLog = eventLog;

    [HttpPost("image")]
    public Task SetImage(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] DeployableId deployableId,
        [FromBody] DeployableImageName deployableImageName) => _eventLog.Append(deployableId, new DeployableImageChanged(applicationId, environmentId, microserviceId, deployableId, deployableImageName));

    [HttpPost("environment-variables")]
    public Task SetEnvironmentVariableForDeployable(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] DeployableId deployableId,
        [FromBody] EnvironmentVariable environmentVariable) =>
        _eventLog.Append(deployableId, new EnvironmentVariableSetForDeployable(environmentId, microserviceId, deployableId, environmentVariable.Key, environmentVariable.Value));

    [HttpPost("secrets")]
    public Task SetSecretForDeployable(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] DeployableId deployableId,
        [FromBody] Secret secret) =>
        _eventLog.Append(deployableId, new SecretSetForDeployable(environmentId, microserviceId, deployableId, secret.Key, secret.Value));
}
