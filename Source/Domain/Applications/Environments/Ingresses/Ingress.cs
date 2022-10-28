// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Concepts.Applications.Environments.Ingresses;
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

    [HttpPost("route")]
    public Task DefineRouteForIngress(
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

    [HttpPost("custom-domains")]
    public Task AddCustomDomainToIngress(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] IngressId ingressId,
        [FromBody] AddCustomDomainToIngress command) =>
         _eventLog.Append(ingressId, new CustomDomainAddedToIngress(command.Domain, command.CertificateId));
}
