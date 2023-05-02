// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Events.Applications.Microservices.Modules.Deployables;

[EventType("8f656b91-d64b-4544-ac97-7963ae274af0")]
public record DeployableImageChanged(
    ApplicationId ApplicationId,
    MicroserviceId MicroserviceId,
    ModuleId ModuleId,
    DeployableId DeployableId,
    DeployableImageName Image);
