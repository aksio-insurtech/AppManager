// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Events.Applications.Environments.Microservices;

[EventType("99a650d8-3027-405b-ad80-6bbc47887647")]
public record MicroserviceCreated(MicroserviceId MicroserviceId, MicroserviceName Name);
