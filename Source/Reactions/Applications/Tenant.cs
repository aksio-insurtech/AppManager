// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Execution;
using Concepts.Applications.Tenants;

namespace Reactions.Applications;

public record Tenant(
    TenantId Id,
    TenantName Name,
    IEnumerable<TenantIdentityProviderConfig> IdentityProviders,
    IEnumerable<string>? SourceIdentifiers);
