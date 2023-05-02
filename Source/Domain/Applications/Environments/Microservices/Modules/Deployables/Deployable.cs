// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Events.Applications.Environments.Microservices.Modules.Deployables;

namespace Domain.Applications.Environments.Microservices.Modules.Deployables;

[Route("/api/applications/{applicationId}/environments/{environmentId}/microservices/{microserviceId}/modules/{moduleId}/deployables/{deployableId}")]
public class Deployable : Controller
{
    readonly IEventLog _eventLog;

    public Deployable(IEventLog eventLog) => _eventLog = eventLog;

    [HttpPost("image")]
    public Task SetImage(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] ModuleId moduleId,
        [FromRoute] DeployableId deployableId,
        [FromBody] DeployableImageName deployableImageName) => _eventLog.Append(environmentId, new DeployableImageChangedInEnvironment(applicationId, environmentId, microserviceId, moduleId, ApplicationEnvironmentDeploymentId.New(), deployableId, deployableImageName));

    [HttpPost("config-files")]
    public Task SetConfigFileForDeployable(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] ModuleId moduleId,
        [FromRoute] DeployableId deployableId,
        [FromBody] ConfigFile configFile) =>
        _eventLog.Append(environmentId, new ConfigFileSetForDeployable(applicationId, environmentId, microserviceId, moduleId, deployableId, configFile.Name, configFile.Content));

    [HttpPost("environment-variables")]
    public Task SetEnvironmentVariableForDeployable(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] ModuleId moduleId,
        [FromRoute] DeployableId deployableId,
        [FromBody] EnvironmentVariable environmentVariable) =>
        _eventLog.Append(deployableId, new EnvironmentVariableSetForDeployable(applicationId, environmentId, microserviceId, moduleId, deployableId, environmentVariable.Key, environmentVariable.Value));

    [HttpPost("secrets")]
    public Task SetSecretForDeployable(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] ModuleId moduleId,
        [FromRoute] DeployableId deployableId,
        [FromBody] Secret secret) =>
        _eventLog.Append(deployableId, new SecretSetForDeployable(applicationId, environmentId, microserviceId, moduleId, deployableId, secret.Key, secret.Value));
}
