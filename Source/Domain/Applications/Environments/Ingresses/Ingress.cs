// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Concepts.Applications.Ingresses;
using Events.Applications.Environments.Ingresses;

namespace Domain.Applications.Environments.Ingresses;

[Route("/api/applications/{applicationId}/environments/{environmentId}/ingresses/{ingressId}")]
public class Ingress : Controller
{
    readonly IEventLog _eventLog;

    public Ingress(IEventLog eventLog)
    {
        _eventLog = eventLog;
    }

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

    [HttpPost("define-route")]
    public Task DefineRoute(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] IngressId ingressId,
        [FromBody] DefineRoute @command) =>
        _eventLog.Append(ingressId, new RouteDefinedOnIngress(
            applicationId,
            environmentId,
            ingressId,
            @command.Path,
            @command.TargetMicroservice,
            @command.TargetPath));
}
