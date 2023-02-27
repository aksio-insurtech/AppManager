// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Events.Applications;
using Events.Applications.Environments;

namespace Domain.Applications;

[Route("/api/applications/{applicationId}")]
public class Application : Controller
{
    readonly IEventLog _eventLog;

    public Application(IEventLog eventLog)
    {
        _eventLog = eventLog;
    }

    [HttpPost("config-files")]
    public Task SetConfigFileForApplication(
        [FromRoute] ApplicationId applicationId,
        [FromBody] ConfigFile configFile) =>
        _eventLog.Append(applicationId, new ConfigFileSetForApplication(configFile.Name, configFile.Content));

    [HttpPost("environment-variables")]
    public Task SetEnvironmentVariableForApplication([FromRoute] ApplicationId applicationId, [FromBody] EnvironmentVariable environmentVariable) =>
        _eventLog.Append(applicationId, new EnvironmentVariableSetForApplication(environmentVariable.Key, environmentVariable.Value));

    [HttpPost("secrets")]
    public Task SetSecretForApplication([FromRoute] ApplicationId applicationId, [FromBody] Secret secret) =>
        _eventLog.Append(applicationId, new SecretSetForApplication(secret.Key, secret.Value));

    [HttpPost("add-environment/{environmentId}")]
    public Task AddEnvironmentToApplication([FromRoute] ApplicationId applicationId, [FromRoute] ApplicationEnvironmentId environmentId) =>
        _eventLog.Append(applicationId, new ApplicationEnvironmentAddedToApplication(environmentId));
}
