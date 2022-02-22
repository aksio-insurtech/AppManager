// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Events.Applications;

[EventType("e3e55a84-ada3-4147-b048-c178caf19dea")]
public record DeployableImageChanged(DeployableImageName ImageName);
