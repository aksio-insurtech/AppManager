// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Events.Applications.Environments.Microservices.Modules.Deployables;

[EventType("5b8b0626-58b9-454e-bce2-a6564baec53d")]
public record SecretSetForDeployable(
    ApplicationId ApplicationId,
    ApplicationEnvironmentId EnvironmentId,
    MicroserviceId MicroserviceId,
    ModuleId ModuleId,
    DeployableId DeployableId,
    SecretKey Key,
    SecretValue Value);
