// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Domain.Applications;

[Route("/api/applications/{applicationId}")]
public class Application : Controller
{
    [HttpPost("config")]
    public Task SetConfigForApplication([FromRoute] ApplicationId applicationId, ConfigFile configFile) => Task.CompletedTask;

    [HttpPost("environment-variable")]
    public Task SetEnvironmentVariableForApplication([FromRoute] ApplicationId applicationId, EnvironmentVariable environmentVariable) => Task.CompletedTask;

    [HttpPost("secret")]
    public Task SetSecretForApplication([FromRoute] ApplicationId applicationId, Secret secret) => Task.CompletedTask;
}
