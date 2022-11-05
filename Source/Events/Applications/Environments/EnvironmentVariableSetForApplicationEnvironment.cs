// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Events.Applications.Environments;

[EventType("faaa4060-17c1-4b5b-8a27-550e369628c1")]
public record EnvironmentVariableSetForApplicationEnvironment(ApplicationId ApplicationId, ApplicationEnvironmentId EnvironmentId, EnvironmentVariableKey Key, EnvironmentVariableValue Value);
