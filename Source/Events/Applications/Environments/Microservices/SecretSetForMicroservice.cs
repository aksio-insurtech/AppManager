// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Events.Applications.Environments.Microservices;

[EventType("8cfaf97d-8993-4cef-a383-8e2358cb4d60")]
public record SecretSetForMicroservice(MicroserviceId MicroserviceId, ApplicationEnvironmentId EnvironmentId, SecretKey Key, SecretValue Value);
