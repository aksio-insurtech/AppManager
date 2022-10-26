// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Events.Applications;
using Events.Applications.Environments;

namespace Domain.Applications.Environments;

[Route("/api/applications/{applicationId}/environments/{environmentId}")]
public class ApplicationEnvironment : Controller
{
    readonly IEventLog _eventLog;

    public ApplicationEnvironment(IEventLog eventLog)
    {
        _eventLog = eventLog;
    }

    [HttpPost("custom-domain")]
    public Task AddCustomDomainToApplicationEnvironment(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromBody] AddCustomDomainToApplicationEnvironment command) =>
         _eventLog.Append(environmentId, new CustomDomainAddedToApplicationEnvironment(command.Domain, command.Certificate));

    [HttpPost("config")]
    public Task SetConfig([FromRoute] ApplicationId applicationId, [FromRoute] ApplicationEnvironmentId environmentId) => Task.CompletedTask;

    [HttpPost("environment-variable")]
    public Task SetEnvironmentVariableForApplicationEnvironment(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromBody] EnvironmentVariable environmentVariable) =>
        _eventLog.Append(environmentId, new EnvironmentVariableSetForApplicationEnvironment(applicationId, environmentVariable.Key, environmentVariable.Value));
}
