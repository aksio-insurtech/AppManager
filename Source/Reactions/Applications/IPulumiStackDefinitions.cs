// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Pulumi.Automation;

namespace Reactions.Applications;

public interface IPulumiStackDefinitions
{
    PulumiFn Application(Application application, CloudRuntimeEnvironment environment);
    PulumiFn Microservice(Application application, Microservice microservice, CloudRuntimeEnvironment environment);
    PulumiFn Deployable(Application application, Microservice microservice, Deployable deployable, CloudRuntimeEnvironment environment);
}
