// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Events.Applications.Environments.Microservices.Modules;

namespace Domain.Applications.Environments.Microservices.Modules;

[Route("/api/applications/{applicationId}/environments/{environmentId}/microservices/{microserviceId}/modules")]
public class Modules : Controller
{
    readonly IEventLog _eventLog;

    public Modules(IEventLog eventLog) => _eventLog = eventLog;

    [HttpPost]
    public Task CreateModule(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] ModuleId moduleId,
        [FromBody] CreateModule command) => _eventLog.Append(environmentId, new ModuleCreated(environmentId, microserviceId, command.ModuleId, command.Name));
}
