// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Events.Applications.Environments.Microservices.Modules.Deployables;

[EventType("04e77fbb-7149-4848-9b4b-ab3ea07cbd26")]
public record ConfigFileSetForDeployable(
    ApplicationId ApplicationId,
    ApplicationEnvironmentId EnvironmentId,
    MicroserviceId MicroserviceId,
    ModuleId ModuleId,
    DeployableId DeployableId,
    ConfigFileName Name,
    ConfigFileContent Content);
