// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Pulumi.Automation;

namespace Infrastructure;

public interface IStacksForMicroservices
{
    Task<bool> HasFor(ApplicationId applicationId, MicroserviceId microserviceId, ApplicationEnvironment environment);
    Task<StackDeployment> GetFor(ApplicationId applicationId, MicroserviceId microserviceId, ApplicationEnvironment environment);
    Task Save(ApplicationId applicationId, MicroserviceId microserviceId, ApplicationEnvironment environment, StackDeployment stackDeployment);
}
