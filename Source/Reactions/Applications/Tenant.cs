// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Concepts.Applications.Tenants;

namespace Reactions.Applications;

public record Tenant(
    TenantId Id,
    TenantName Name,
    Domain? Domain,
    OnBehalfOf? OnBehalfOf);
