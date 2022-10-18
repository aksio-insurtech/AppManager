// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments;

namespace Domain.Applications.Environments;

[Route("/api/applications/{applicationId}/environments/{environment}")]
public class Environments : Controller
{
    readonly IEventLog _eventLog;

    public Environments(IEventLog eventLog)
    {
        _eventLog = eventLog;
    }

    [HttpPost]
    public Task CreateEnvironment([FromRoute] ApplicationId applicationId, [FromBody] CreateEnvironment addEnvironment) =>
            _eventLog.Append(addEnvironment.EnvironmentId, new EnvironmentCreated(applicationId, addEnvironment.Name, addEnvironment.DisplayName, addEnvironment.ShortName));
}
