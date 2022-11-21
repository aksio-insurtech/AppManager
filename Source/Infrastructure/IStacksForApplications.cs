// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Pulumi.Automation;

namespace Infrastructure;

public interface IStacksForApplications
{
    Task<bool> HasFor(ApplicationId applicationId, ApplicationEnvironment environment);
    Task<StackDeployment> GetFor(ApplicationId applicationId, ApplicationEnvironment environment);
    Task Save(ApplicationId applicationId, ApplicationEnvironment environment, StackDeployment stackDeployment);
}
