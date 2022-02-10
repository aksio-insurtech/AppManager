// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.Automation;

namespace Reactions.Applications
{
    public interface IPulumiStackDefinitions
    {
        PulumiFn CreateApplication(Application application, RuntimeEnvironment environment);
        PulumiFn CreateMicroservice(RuntimeEnvironment environment);
        PulumiFn CreateDeployable(RuntimeEnvironment environment);
    }
}
