// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;

namespace Domain.Applications;

[Route("/api/applications")]
public class Applications : Controller
{
    readonly IEventLog _eventLog;

    public Applications(IEventLog eventLog)
    {
        _eventLog = eventLog;
    }

    [HttpPost]
    public Task Create([FromBody] CreateApplication command) =>
         _eventLog.Append(command.ApplicationId, new ApplicationCreated(command.Name, command.AzureSubscriptionId, command.CloudLocation));
}
