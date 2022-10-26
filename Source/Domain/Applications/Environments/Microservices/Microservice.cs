// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Events.Applications.Environments.Microservices;
using Infrastructure;
using Pulumi.Automation;

namespace Domain.Applications.Environments.Microservices;

[Route("/api/applications/{applicationId}/{environment}/microservices/{microserviceId}")]
public class Microservice : Controller
{
    readonly IEventLog _eventLog;
    readonly IStacksForMicroservices _stacksForMicroservices;

    public Microservice(
        IEventLog eventLog,
        IStacksForMicroservices stacksForMicroservices)
    {
        _eventLog = eventLog;
        _stacksForMicroservices = stacksForMicroservices;
    }

    [HttpPost("config")]
    public Task SetConfig(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] string environment,
        [FromRoute] MicroserviceId microserviceId) => Task.CompletedTask;

    [HttpPost("environment-variable")]
    public Task SetEnvironmentVariable(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] string environment,
        [FromRoute] MicroserviceId microserviceId) => Task.CompletedTask;

    [HttpPost("stack")]
    public Task SetStack(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] Concepts.Applications.Environments.ApplicationEnvironment environment,
        [FromBody] object stack) => _stacksForMicroservices.Save(applicationId, microserviceId, environment, StackDeployment.FromJsonString(stack.ToString()!));

    [HttpPost("remove")]
    public Task Remove([FromRoute] MicroserviceId microserviceId) => _eventLog.Append(microserviceId, new MicroserviceRemoved());
}
