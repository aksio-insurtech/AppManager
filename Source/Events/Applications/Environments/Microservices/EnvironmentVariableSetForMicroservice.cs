// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Events.Applications.Environments.Microservices;

[EventType("af5f1b91-9c73-4692-8a86-2ba1f1a8e082")]
public record EnvironmentVariableSetForMicroservice(ApplicationId ApplicationId, ApplicationEnvironmentId EnvironmentId, EnvironmentVariableKey Key, EnvironmentVariableValue Value);
