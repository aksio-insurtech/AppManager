// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Concepts.Security;

namespace Reactions.Applications;

public record IdentityProviderImpersonation(
    IEnumerable<TenantId> Tenants,
    IEnumerable<Role> Roles,
    IEnumerable<Group> Groups,
    IEnumerable<Claim> Claims);
