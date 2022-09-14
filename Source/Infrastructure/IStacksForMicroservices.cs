// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Concepts.Applications;
using Pulumi.Automation;

namespace Infrastructure;

public interface IStacksForMicroservices
{
    Task<bool> HasFor(MicroserviceId microserviceId, CloudRuntimeEnvironment environment);
    Task<StackDeployment> GetFor(MicroserviceId microserviceId, CloudRuntimeEnvironment environment);
    Task Save(MicroserviceId microserviceId, CloudRuntimeEnvironment environment, StackDeployment stackDeployment);
}
