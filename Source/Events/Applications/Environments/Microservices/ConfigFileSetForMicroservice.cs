// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Events.Applications.Environments.Microservices;

[EventType("3486374d-968e-437c-b142-9d09117932fd")]
public record ConfigFileSetForMicroservice(MicroserviceId MicroserviceId, ApplicationEnvironmentId EnvironmentId, ConfigFileName Name, ConfigFileContent Content);
