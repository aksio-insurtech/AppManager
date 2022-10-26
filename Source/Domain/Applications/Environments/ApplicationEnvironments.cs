// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Events.Applications.Environments;
using Infrastructure;

namespace Domain.Applications.Environments;

[Route("/api/applications/{applicationId}/environments")]
public class ApplicationEnvironments : Controller
{
    readonly IEventLog _eventLog;
    readonly IDockerHub _dockerHub;

    public ApplicationEnvironments(IEventLog eventLog, IDockerHub dockerHub)
    {
        _eventLog = eventLog;
        _dockerHub = dockerHub;
    }

    [HttpPost]
    public async Task CreateApplicationEnvironment([FromRoute] ApplicationId applicationId, [FromBody] CreateApplicationEnvironment addEnvironment)
    {
        var cratisVersion = await _dockerHub.GetLastVersionOfCratis();
        await _eventLog.Append(
            addEnvironment.EnvironmentId,
            new ApplicationEnvironmentCreated(
                applicationId,
                addEnvironment.Name,
                addEnvironment.DisplayName,
                addEnvironment.ShortName,
                cratisVersion,
                addEnvironment.AzureSubscriptionId,
                addEnvironment.CloudLocation));

        await _eventLog.Append(TenantId.Development.Value, new TenantAddedToApplicationEnvironment(addEnvironment.EnvironmentId, TenantId.Development, "Development", "development"));
    }
}
