// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Events.Applications;

namespace Domain.Applications.Environments.Ingresses;

[Route("/api/applications/{applicationId}/environments/{environmentId}/ingresses")]
public class Ingresses : Controller
{
    readonly IEventLog _eventLog;

    public Ingresses(IEventLog eventLog) => _eventLog = eventLog;

    [HttpPost]
    public Task CreateIngress(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromBody] CreateIngress createIngress) =>
        _eventLog.Append(createIngress.IngressId, new IngressCreated(applicationId, environmentId, createIngress.Name));
}
