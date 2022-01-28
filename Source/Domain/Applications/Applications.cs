// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;

namespace Domain.Applications
{
    [Route("/api/applications")]
    public class Applications : Controller
    {
        readonly IEventLog _eventLog;

        public Applications(IEventLog eventLog) => _eventLog = eventLog;

        [HttpPost]
        public Task Create([FromBody] CreateApplication command) => _eventLog.Append(command.ApplicationId.ToString(), new ApplicationCreated(command.Name, command.CloudLocation));

        [HttpPost("remove")]
        public Task Remove([FromBody] RemoveApplication command) => _eventLog.Append(command.ApplicationId.ToString(), new ApplicationRemoved());
    }
}
