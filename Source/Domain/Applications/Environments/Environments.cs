// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
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
    public async Task CreateEnvironment([FromRoute] ApplicationId applicationId, [FromBody] CreateEnvironment addEnvironment)
    {
        await _eventLog.Append(addEnvironment.EnvironmentId, new EnvironmentCreated(applicationId, addEnvironment.Name, addEnvironment.DisplayName, addEnvironment.ShortName));
        await _eventLog.Append(addEnvironment.EnvironmentId, new TenantAddedToApplicationEnvironment(addEnvironment.EnvironmentId, TenantId.Development, "Development"));
    }
}
