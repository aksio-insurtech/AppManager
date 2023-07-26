// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Execution;
using Concepts.Security;
using MicroserviceId = Concepts.Applications.MicroserviceId;

namespace Reactions.Applications;

public record IdentityProviderImpersonation(
    MicroserviceId TargetMicroservice,
    IEnumerable<TenantId> Tenants,
    IEnumerable<Role> Roles,
    IEnumerable<Group> Groups,
    IEnumerable<Claim> Claims);
