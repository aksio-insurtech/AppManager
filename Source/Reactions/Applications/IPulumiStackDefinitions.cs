// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Pulumi.Automation;

namespace Reactions.Applications
{
    public interface IPulumiStackDefinitions
    {
        PulumiFn CreateApplication(Application application, CloudRuntimeEnvironment environment);
        PulumiFn CreateMicroservice(Application application, Microservice microservice, CloudRuntimeEnvironment environment);
        PulumiFn CreateDeployable(CloudRuntimeEnvironment environment);
    }
}
