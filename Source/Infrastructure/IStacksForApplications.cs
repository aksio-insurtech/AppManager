// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Pulumi.Automation;

namespace Infrastructure;

public interface IStacksForApplications
{
    Task<bool> HasFor(ApplicationId applicationId, CloudRuntimeEnvironment environment);
    Task<StackDeployment> GetFor(ApplicationId applicationId, CloudRuntimeEnvironment environment);
    Task Save(ApplicationId applicationId, CloudRuntimeEnvironment environment, StackDeployment stackDeployment);
}
