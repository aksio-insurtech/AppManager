// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Events.Applications;

namespace Domain.Applications;

[Route("/api/applications/{applicationId}")]
public class Application : Controller
{
    readonly IEventLog _eventLog;

    public Application(IEventLog eventLog)
    {
        _eventLog = eventLog;
    }

    [HttpPost("config")]
    public Task SetConfigFileForApplication(
        [FromRoute] ApplicationId applicationId,
        [FromBody] ConfigFile configFile) =>
        _eventLog.Append(applicationId, new ConfigFileSetForApplication(configFile.Name, configFile.Content));

    [HttpPost("environment-variable")]
    public Task SetEnvironmentVariableForApplication([FromRoute] ApplicationId applicationId, [FromBody] EnvironmentVariable environmentVariable) =>
        _eventLog.Append(applicationId, new EnvironmentVariableSetForApplication(environmentVariable.Key, environmentVariable.Value));

    [HttpPost("secret")]
    public Task SetSecretForApplication([FromRoute] ApplicationId applicationId, Secret secret) => Task.CompletedTask;
}
