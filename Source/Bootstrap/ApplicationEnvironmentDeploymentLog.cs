// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Subjects;
using Concepts.Applications.Environments;
using Read.Applications.Environments;

namespace Bootstrap;

public class ApplicationEnvironmentDeploymentLog : IApplicationEnvironmentDeploymentLog
{
    /// <inheritdoc/>
    public void Append(ApplicationId applicationId, ApplicationEnvironmentId environmentId, ApplicationEnvironmentDeploymentId deploymentId, string message)
    {
    }

    /// <inheritdoc/>
    public IObservable<string> LogFor(ApplicationId applicationId, ApplicationEnvironmentId environmentId, ApplicationEnvironmentDeploymentId deploymentId) => new Subject<string>();
}
