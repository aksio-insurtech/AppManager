// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Events.Applications.Environments.Microservices.Deployables;

[EventType("12e6d722-1e91-4802-86db-00ff3d61d798")]
public record DeployableCreated(ApplicationEnvironmentId EnvironmentId, MicroserviceId MicroserviceId, DeployableId DeployableId, DeployableName Name);
