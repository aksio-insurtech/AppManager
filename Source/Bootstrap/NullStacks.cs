// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.Automation;
using Reactions.Applications;
using Reactions.Applications.Pulumi;

namespace Bootstrap;

public class NullStacks : IStacks
{
    public Task<StackDeployment> GetFor(Application application) => Task.FromResult(StackDeployment.FromJsonString("{}"));

    public Task<bool> HasFor(Application application) => Task.FromResult(false);

    public Task Save(Application application, StackDeployment stackDeployment) => Task.CompletedTask;
}
