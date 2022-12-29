// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Events.Applications.Environments.Microservices.Deployables;

[EventType("5b8b0626-58b9-454e-bce2-a6564baec53d")]
public record SecretSetForDeployable(ApplicationEnvironmentId EnvironmentId, MicroserviceId MicroserviceId, DeployableId DeployableId, SecretKey Key, SecretValue Value);
