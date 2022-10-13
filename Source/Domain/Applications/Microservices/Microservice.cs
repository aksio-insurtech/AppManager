// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Concepts.Applications;
using Infrastructure;
using Pulumi.Automation;

namespace Domain.Applications;

[Route("/api/applications/{applicationId}/{environment}/microservices/{microserviceId}")]
public class Microservice : Controller
{
    readonly IStacksForMicroservices _stacksForMicroservices;

    public Microservice(IStacksForMicroservices stacksForMicroservices)
    {
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

    [HttpPost("{microserviceId}/stack/{environment}")]
    public Task SetStack(
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] CloudRuntimeEnvironment environment,
        [FromBody] object stack) => _stacksForMicroservices.Save(microserviceId, environment, StackDeployment.FromJsonString(stack.ToString()!));
}
