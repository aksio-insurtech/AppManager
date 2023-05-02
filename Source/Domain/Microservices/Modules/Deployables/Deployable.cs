// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Events.Applications.Microservices.Modules.Deployables;

namespace Domain.Applications.Microservices.Modules.Deployables;

[Route("/api/applications/{applicationId}/microservices/{microserviceId}/module/{moduleId}/deployables/{deployableId}")]
public class Deployable : Controller
{
    readonly IEventLog _eventLog;

    public Deployable(IEventLog eventLog) => _eventLog = eventLog;

    [HttpPost("image")]
    public Task SetImage(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] ModuleId moduleId,
        [FromRoute] DeployableId deployableId,
        [FromBody] DeployableImageName deployableImageName) => _eventLog.Append(applicationId, new DeployableImageChanged(applicationId, microserviceId, moduleId, deployableId, deployableImageName));
}
