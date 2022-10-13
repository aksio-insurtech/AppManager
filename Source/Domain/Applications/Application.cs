// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Domain.Applications;

[Route("/api/applications/{applicationId}")]
public class Application : Controller
{
    [HttpPost("environment")]
    public Task AddEnvironment([FromRoute] ApplicationId applicationId) => Task.CompletedTask;
}
