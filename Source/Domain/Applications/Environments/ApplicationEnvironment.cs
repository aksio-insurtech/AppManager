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

    [HttpPost("app-settings")]
    public Task SetAppSettingsForApplicationEnvironment(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromBody] AppSettings appSettings) =>
        _eventLog.Append(environmentId, new AppSettingsSetForApplicationEnvironment(applicationId, environmentId, appSettings.Content));

    [HttpPost("config-files")]
    public Task SetConfigFileForApplicationEnvironment(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromBody] ConfigFile configFile) =>
        _eventLog.Append(environmentId, new ConfigFileSetForApplicationEnvironment(applicationId, environmentId, configFile.Name, configFile.Content));

    [HttpPost("environment-variables")]
    public Task SetEnvironmentVariableForApplicationEnvironment(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromBody] EnvironmentVariable environmentVariable) =>
        _eventLog.Append(environmentId, new EnvironmentVariableSetForApplicationEnvironment(applicationId, environmentId, environmentVariable.Key, environmentVariable.Value));

    [HttpPost("secrets")]
    public Task SetSecretForApplicationEnvironment(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromBody] Secret secret) =>
        _eventLog.Append(environmentId, new SecretSetForApplicationEnvironment(applicationId, environmentId, secret.Key, secret.Value));

    [HttpPost("certificates")]
    public Task AddCertificateToApplicationEnvironment(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromBody] AddCertificateToApplicationEnvironment command) =>
        _eventLog.Append(environmentId, new CertificateAddedToApplicationEnvironment(applicationId, environmentId, command.CertificateId, command.Name, command.Certificate));

    [HttpPost("consolidate")]
    public Task ConsolidateApplicationEnvironment(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId) =>
        _eventLog.Append(environmentId, new ApplicationEnvironmentDeploymentStarted(applicationId, environmentId, ApplicationEnvironmentDeploymentId.New()));
}
