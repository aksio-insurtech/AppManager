// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Concepts.Applications;
using Events.Applications;
using Infrastructure;
using Pulumi.Automation;

namespace Domain.Applications;

[Route("/api/applications/{applicationId}/microservices")]
public class Microservices : Controller
{
    readonly IEventLog _eventLog;
    readonly IStacksForMicroservices _stacksForMicroservices;

    public Microservices(IEventLog eventLog, IStacksForMicroservices stacksForMicroservices)
    {
        _eventLog = eventLog;
        _stacksForMicroservices = stacksForMicroservices;
    }

    [HttpPost("{microserviceId}/stack/{environment}")]
    public Task SetStack([FromRoute] MicroserviceId microserviceId, [FromRoute] CloudRuntimeEnvironment environment, [FromBody] object stack) => _stacksForMicroservices.Save(microserviceId, environment, StackDeployment.FromJsonString(stack.ToString()!));

    [HttpPost]
    public Task Create([FromRoute] ApplicationId applicationId, [FromBody] CreateMicroservice command) => _eventLog.Append(command.MicroserviceId.ToString(), new MicroserviceCreated(applicationId, command.Name));

    [HttpPost("{microserviceId}/remove")]
    public Task Remove([FromRoute] MicroserviceId microserviceId) => _eventLog.Append(microserviceId.ToString(), new MicroserviceRemoved());
}
