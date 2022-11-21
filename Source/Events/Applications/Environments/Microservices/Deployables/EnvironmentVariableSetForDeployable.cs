// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Events.Applications.Environments.Microservices.Deployables;

[EventType("91021fa2-3bd2-4b04-a4ae-d371827f205f")]
public record EnvironmentVariableSetForDeployable(MicroserviceId MicroserviceId, DeployableId DeployableId, EnvironmentVariableKey Key, EnvironmentVariableValue Value);
