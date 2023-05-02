// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Events.Applications.Environments.Microservices.Modules;

[EventType("e3e0e2ec-c417-481a-97cb-063d825c2066")]
public record ModuleRemoved(ModuleId ModuleId);
