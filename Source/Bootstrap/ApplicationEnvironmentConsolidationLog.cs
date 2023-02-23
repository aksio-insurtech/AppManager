// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reactive.Subjects;
using Concepts.Applications.Environments;
using Read.Applications.Environments;

namespace Bootstrap;

public class ApplicationEnvironmentConsolidationLog : IApplicationEnvironmentConsolidationLog
{
    /// <inheritdoc/>
    public void Append(ApplicationId applicationId, ApplicationEnvironmentId environmentId, ApplicationEnvironmentConsolidationId consolidationId, string message)
    {
    }

    /// <inheritdoc/>
    public IObservable<string> LogFor(ApplicationId applicationId, ApplicationEnvironmentId environmentId, ApplicationEnvironmentConsolidationId consolidationId) => new Subject<string>();
}
