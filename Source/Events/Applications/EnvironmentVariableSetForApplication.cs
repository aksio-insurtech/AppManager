// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Events.Applications;

[EventType("cd7b1761-9a3e-4b8f-8857-f37e54f76678")]
public record EnvironmentVariableSetForApplication(EnvironmentVariableKey Key, EnvironmentVariableValue Value);
