// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Execution;
using Concepts.Applications.Tenants;

namespace Events.Applications.Environments;

[EventType("8600851a-6143-4a7b-a255-33a33039d2fe")]
public record TenantAddedToApplicationEnvironment(TenantId TenantId, TenantName Name);
