// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Domain.Applications;

[Route("/api/applications/{applicationId}/{environment}")]
public class ApplicationEnvironment : Controller
{
    [HttpPost("config")]
    public Task SetConfig([FromRoute] ApplicationId applicationId, [FromRoute] string environment) => Task.CompletedTask;

    [HttpPost("environment-variable")]
    public Task SetEnvironmentVariable([FromRoute] ApplicationId applicationId, [FromRoute] string environment) => Task.CompletedTask;

    [HttpPost("ingress")]
    public Task AddIngress([FromRoute] ApplicationId applicationId, [FromRoute] string environment) => Task.CompletedTask;

    [HttpPost("ingress")]
    public Task AddMicroservice([FromRoute] ApplicationId applicationId, [FromRoute] string environment) => Task.CompletedTask;
}
