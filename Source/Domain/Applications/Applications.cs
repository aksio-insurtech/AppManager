// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;
using Infrastructure;
using MongoDB.Bson;
using Pulumi.Automation;

namespace Domain.Applications;

[Route("/api/applications")]
public class Applications : Controller
{
    readonly IEventLog _eventLog;
    readonly IStacks _stacks;

    public Applications(IEventLog eventLog, IStacks stacks)
    {
        _eventLog = eventLog;
        _stacks = stacks;
    }

    [HttpPost("{applicationId}/stack")]
    public Task SetStack([FromRoute] ApplicationId applicationId, [FromBody] object stack) => _stacks.Save(applicationId, StackDeployment.FromJsonString(stack.ToString()!));

    [HttpPost]
    public Task Create([FromBody] CreateApplication command) => _eventLog.Append(command.ApplicationId.ToString(), new ApplicationCreated(command.Name, command.AzureSubscriptionId, command.CloudLocation));

    [HttpPost("{applicationId}/authentication")]
    public Task ConfigureAuthentication([FromRoute] ApplicationId applicationId, [FromBody] ConfigureAuthentication command) => _eventLog.Append(applicationId.ToString(), new AuthenticationConfiguredForApplication(command.ClientId, command.ClientSecret));

    [HttpPost("{applicationId}/remove")]
    public Task Remove([FromRoute] ApplicationId applicationId) => _eventLog.Append(applicationId.ToString(), new ApplicationRemoved());
}
