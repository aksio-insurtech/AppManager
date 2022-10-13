// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Domain.Applications.Ingresses;

[Route("/api/applications/{applicationId}/{environment}/ingresses/{ingressId}")]
public class Ingresses
{
    [HttpPost("authentication")]
    public Task ConfigureAuthentication([FromRoute] ApplicationId applicationId, [FromBody] ConfigureAuthentication command) => Task.CompletedTask;

    [HttpPost("route")]
    public Task SetRoute([FromRoute] ApplicationId applicationId) => Task.CompletedTask;

    [HttpPost("{microserviceId}")]
    public Task MakeFrontend([FromRoute] ApplicationId applicationId, [FromRoute] MicroserviceId microserviceId) => Task.CompletedTask;
}
