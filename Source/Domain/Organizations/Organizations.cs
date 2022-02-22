// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Organizations;

namespace Domain.Organizations;

[Route("/api/organizations")]
public class Organizations : Controller
{
    readonly IEventLog _eventLog;

    public Organizations(IEventLog eventLog) => _eventLog = eventLog;

    [HttpPost]
    public Task Register([FromBody] RegisterOrganization command) => _eventLog.Append(command.Id.ToString(), new OrganizationRegistered(command.Name));
}
