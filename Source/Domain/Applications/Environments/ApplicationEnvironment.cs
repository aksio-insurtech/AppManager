// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
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

    [HttpPost("config")]
    public Task SetConfig([FromRoute] ApplicationId applicationId, [FromRoute] ApplicationEnvironmentId environmentId) => Task.CompletedTask;

    [HttpPost("environment-variables")]
    public Task SetEnvironmentVariableForApplicationEnvironment(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromBody] EnvironmentVariable environmentVariable) =>
        _eventLog.Append(applicationId, new EnvironmentVariableSetForApplicationEnvironment(applicationId, environmentId, environmentVariable.Key, environmentVariable.Value));

    [HttpPost("certificates")]
    public Task AddCertificateToApplicationEnvironment(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromBody] AddCertificateToApplicationEnvironment command) =>
        _eventLog.Append(applicationId, new CertificateAddedToApplicationEnvironment(applicationId, environmentId, command.CertificateId, command.Name, command.Certificate));

    [HttpPost("consolidate")]
    public Task ConsolidateApplicationEnvironment(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId) =>
        _eventLog.Append(applicationId, new ApplicationEnvironmentConsolidationStarted(applicationId, environmentId, ApplicationEnvironmentConsolidationId.New()));
}
