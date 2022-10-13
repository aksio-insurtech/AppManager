// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Events.Applications;

namespace Domain.Applications;

[Route("/api/applications/{applicationId}/{environment}/microservices")]
public class Microservices : Controller
{
    readonly IEventLog _eventLog;

    public Microservices(IEventLog eventLog)
    {
        _eventLog = eventLog;
    }

    [HttpPost]
    public Task Create(
        [FromRoute] ApplicationId applicationId,
        [FromBody] CreateMicroservice command) => _eventLog.Append(command.MicroserviceId, new MicroserviceCreated(applicationId, command.Name));

    [HttpPost("{microserviceId}/remove")]
    public Task Remove([FromRoute] MicroserviceId microserviceId) => _eventLog.Append(microserviceId, new MicroserviceRemoved());
}
