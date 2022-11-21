// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Concepts.Applications.Environments;
using Concepts.Applications.Tenants;
using Events.Applications.Environments;

namespace Domain.Applications.Environments.Tenants;

[Route("/api/applications/{applicationId}/environments/{environmentId}/tenants/{tenantId}")]
public class Tenant : Controller
{
    readonly IEventLog _eventLog;

    public Tenant(IEventLog eventLog) => _eventLog = eventLog;

    [HttpPost("custom-domain")]
    public Task AssociateDomainWithTenant(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] TenantId tenantId,
        [FromBody] AssociateDomainWithTenant command) =>
        _eventLog.Append(environmentId, new DomainAssociatedWithTenant(tenantId, command.Domain, command.CertificateId));

    [HttpPost("custom-domain")]
    public Task SetOnBehalfOf(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] TenantId tenantId,
        [FromBody] OnBehalfOf onBehalfOf) =>
        _eventLog.Append(environmentId, new OnBehalfOfSetForTenant(tenantId, onBehalfOf));
}
