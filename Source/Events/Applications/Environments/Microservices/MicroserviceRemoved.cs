// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Events.Applications.Environments.Microservices;

[EventType("c294fc88-db33-44c3-be61-4c556644cbbb")]
public record MicroserviceRemoved(MicroserviceId MicroserviceId);
