// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Events.Applications.Environments.Microservices.Modules;

[EventType("0c8da68e-31fb-4463-a7ea-049e548f4957")]
public record ModuleCreated(
    ApplicationEnvironmentId EnvironmentId,
    MicroserviceId MicroserviceId,
    ModuleId ModuleId,
    ModuleName Name);
