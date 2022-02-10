// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.Automation;

namespace Reactions.Applications
{
    public interface IPulumiStacks
    {
        WorkspaceStack CreateApplication(Application application, RuntimeEnvironment environment);
        WorkspaceStack CreateMicroservice(RuntimeEnvironment environment);
        WorkspaceStack CreateDeployable(RuntimeEnvironment environment);
    }
}
