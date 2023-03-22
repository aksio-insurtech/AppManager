// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Events.Applications.Environments.Microservices;
using Infrastructure;
using Pulumi.Automation;

namespace Domain.Applications.Environments.Microservices;

[Route("/api/applications/{applicationId}/environments/{environmentId}/microservices/{microserviceId}")]
public class Microservice : Controller
{
    readonly IEventLog _eventLog;
    readonly IStacksForMicroservices _stacksForMicroservices;

    public Microservice(
        IEventLog eventLog,
        IStacksForMicroservices stacksForMicroservices)
    {
        _eventLog = eventLog;
        _stacksForMicroservices = stacksForMicroservices;
    }

    [HttpPost("app-settings")]
    public Task SetAppSettingsForMicroservice(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromBody] AppSettings appSettings) =>
        _eventLog.Append(environmentId, new AppSettingsSetForMicroservice(applicationId, environmentId, microserviceId, appSettings.Content));

    [HttpPost("config-files")]
    public Task SetConfigFileForMicroservice(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromBody] ConfigFile configFile) =>
        _eventLog.Append(environmentId, new ConfigFileSetForMicroservice(applicationId, environmentId, microserviceId, configFile.Name, configFile.Content));

    [HttpPost("environment-variables")]
    public Task SetEnvironmentVariableForMicroservice(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromBody] EnvironmentVariable environmentVariable) =>
        _eventLog.Append(environmentId, new EnvironmentVariableSetForMicroservice(applicationId, environmentId, microserviceId, environmentVariable.Key, environmentVariable.Value));

    [HttpPost("secrets")]
    public Task SetSecretForMicroservice(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromBody] Secret secret) =>
        _eventLog.Append(microserviceId, new SecretSetForMicroservice(applicationId, environmentId, microserviceId, secret.Key, secret.Value));

    [HttpPost("stack")]
    public Task SetStack(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] Concepts.Applications.Environments.ApplicationEnvironment environment,
        [FromBody] object stack) => _stacksForMicroservices.Save(applicationId, microserviceId, environment, StackDeployment.FromJsonString(stack.ToString()!));

    [HttpPost("remove")]
    public Task Remove(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId) => _eventLog.Append(environmentId, new MicroserviceRemoved(microserviceId));
}
