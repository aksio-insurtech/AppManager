// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Events.Applications.Environments;

[EventType("52cf2936-9549-44f9-9e83-c420c2fc72b8")]
public record ApplicationEnvironmentAddedToApplication(ApplicationEnvironmentId EnvironmentId);
