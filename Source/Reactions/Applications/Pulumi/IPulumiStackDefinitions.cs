// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Pulumi.Automation;

namespace Reactions.Applications.Pulumi;

public interface IPulumiStackDefinitions
{
    PulumiFn Application(ExecutionContext executionContext, Application application, CloudRuntimeEnvironment environment);
    PulumiFn Microservice(ExecutionContext executionContext, Application application, Microservice microservice, CloudRuntimeEnvironment environment);
    PulumiFn Deployable(ExecutionContext executionContext, Application application, Microservice microservice, Deployable deployable, CloudRuntimeEnvironment environment);
}
