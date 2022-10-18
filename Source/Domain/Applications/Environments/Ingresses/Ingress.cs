// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Concepts.Applications.Ingresses;

namespace Domain.Applications.Environments.Ingresses;

[Route("/api/applications/{applicationId}/environments/{environmentId}/ingresses/{ingressId}")]
public class Ingress : Controller
{
    [HttpPost("authentication/openid")]
    public Task ConfigureOpenIDConnectAuthentication(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] IngressId ingressId,
        [FromBody] ConfigureAuthentication command) => Task.CompletedTask;

    [HttpPost("custom-domain")]
    public Task AddCustomDomainForTenant(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] IngressId ingressId,
        [FromBody] AddCustomDomainForTenant command) => Task.CompletedTask;

    [HttpPost("route")]
    public Task SetRoute([FromRoute] ApplicationId applicationId) => Task.CompletedTask;

    [HttpPost("{microserviceId}")]
    public Task MakeFrontend([FromRoute] ApplicationId applicationId, [FromRoute] MicroserviceId microserviceId) => Task.CompletedTask;
}
