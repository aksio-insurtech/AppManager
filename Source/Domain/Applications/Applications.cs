// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Events.Applications;
using Infrastructure;
using Pulumi.Automation;

namespace Domain.Applications;

[Route("/api/applications")]
public class Applications : Controller
{
    readonly IEventLog _eventLog;
    readonly IStacksForApplications _stacksForApplications;

    public Applications(
        IEventLog eventLog,
        IStacksForApplications stacksForApplications)
    {
        _eventLog = eventLog;
        _stacksForApplications = stacksForApplications;
    }

    [HttpPost("{applicationId}/stack/{environment}")]
    public Task SetStack([FromRoute] ApplicationId applicationId, [FromRoute] CloudRuntimeEnvironment environment, [FromBody] object stack) => _stacksForApplications.Save(applicationId, environment, StackDeployment.FromJsonString(stack.ToString()!));

    [HttpPost]
    public Task Create([FromBody] CreateApplication command) => _eventLog.Append(command.ApplicationId.ToString(), new ApplicationCreated(command.Name, command.AzureSubscriptionId, command.CloudLocation));

    [HttpPost("{applicationId}/authentication")]
    public Task ConfigureAuthentication([FromRoute] ApplicationId applicationId, [FromBody] ConfigureAuthentication command) => _eventLog.Append(applicationId.ToString(), new AuthenticationConfiguredForApplication(command.ClientId, command.ClientSecret));

    [HttpPost("{applicationId}/remove")]
    public Task Remove([FromRoute] ApplicationId applicationId) => _eventLog.Append(applicationId.ToString(), new ApplicationRemoved());
}
