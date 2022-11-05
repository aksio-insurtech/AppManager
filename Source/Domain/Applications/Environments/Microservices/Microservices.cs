// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Events.Applications.Environments.Microservices;

namespace Domain.Applications;

[Route("/api/applications/{applicationId}/environments/{environmentId}/microservices")]
public class Microservices : Controller
{
    readonly IEventLog _eventLog;

    public Microservices(IEventLog eventLog)
    {
        _eventLog = eventLog;
    }

    [HttpPost]
    public Task CreateMicroservice(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromBody] CreateMicroservice command) => _eventLog.Append(applicationId, new MicroserviceCreated(applicationId, command.MicroserviceId, environmentId, command.Name));
}
