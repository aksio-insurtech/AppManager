// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Execution;
using Concepts.Applications.Tenants;

namespace Events.Applications.Environments;

[EventType("e6ab276c-8620-4140-b9e2-8d4e60e75918")]
public record OnBehalfOfSetForTenant(TenantId TenantId, OnBehalfOf OnBehalfOf);
