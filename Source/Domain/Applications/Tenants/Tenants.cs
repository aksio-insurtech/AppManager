// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Events.Applications;

namespace Domain.Applications.Tenants;

[Route("/api/applications/{applicationId}/tenants")]
public class Tenants : Controller
{
    readonly IEventLog _eventLog;

    public Tenants(IEventLog eventLog) => _eventLog = eventLog;

    [HttpPost]
    public Task AddTenant(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromBody] AddTenant command) => _eventLog.Append(applicationId, new TenantAddedToApplication(command.TenantId, command.Name));
}
