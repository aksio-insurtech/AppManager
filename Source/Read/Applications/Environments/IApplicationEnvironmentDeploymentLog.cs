// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Read.Applications.Environments;

/// <summary>
/// Defines a log for specific application environment deployment.
/// </summary>
public interface IApplicationEnvironmentDeploymentLog
{
    /// <summary>
    /// Get an observable for a specific log.
    /// </summary>
    /// <param name="applicationId">The identifier of the application.</param>
    /// <param name="environmentId">The identifier of the environment for the application.</param>
    /// <param name="deploymentId">The <see cref="ApplicationEnvironmentDeploymentId"/> the message is for.</param>
    /// <returns><see cref="IObservable{T}"/> with the log.</returns>
    IObservable<string> LogFor(ApplicationId applicationId, ApplicationEnvironmentId environmentId, ApplicationEnvironmentDeploymentId deploymentId);

    /// <summary>
    /// Append log message to the log.
    /// </summary>
    /// <param name="applicationId">The identifier of the application.</param>
    /// <param name="environmentId">The identifier of the environment for the application.</param>
    /// <param name="deploymentId">The <see cref="ApplicationEnvironmentDeploymentId"/> the message is for.</param>
    /// <param name="message">The log message.</param>
    void Append(ApplicationId applicationId, ApplicationEnvironmentId environmentId, ApplicationEnvironmentDeploymentId deploymentId, string message);
}
