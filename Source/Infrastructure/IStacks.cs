// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.Automation;

namespace Infrastructure;

public interface IStacks
{
    Task<bool> HasFor(ApplicationId applicationId);
    Task<StackDeployment> GetFor(ApplicationId applicationId);

    Task Save(ApplicationId applicationId, StackDeployment stackDeployment);
}
