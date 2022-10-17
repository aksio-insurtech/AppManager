// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Domain.Applications.Environments;

[Route("/api/applications/{applicationId}/environments/{environmentId}")]
public class Environment : Controller
{
    [HttpPost("config")]
    public Task SetConfig([FromRoute] ApplicationId applicationId, [FromRoute] ApplicationEnvironmentId environmentId) => Task.CompletedTask;

    [HttpPost("environment-variable")]
    public Task SetEnvironmentVariable([FromRoute] ApplicationId applicationId, [FromRoute] ApplicationEnvironmentId environmentId) => Task.CompletedTask;
}
