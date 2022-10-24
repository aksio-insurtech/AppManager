// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Concepts.Azure;

namespace Reactions.Applications;

public record Application(
    ApplicationId Id,
    ApplicationName Name,
    AzureSubscriptionId AzureSubscriptionId,
    CloudLocationKey CloudLocation,
    ApplicationResources Resources,
    IEnumerable<ApplicationEnvironment> Environments)
{
    public ApplicationEnvironment GetEnvironmentById(ApplicationEnvironmentId id) => Environments.First(_ => _.Id == id);
}
