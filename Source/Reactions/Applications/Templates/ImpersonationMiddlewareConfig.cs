// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Security;

namespace Reactions.Applications.Templates;

public record ImpersonationMiddlewareConfig(
    IEnumerable<string> IdentityProviders,
    IEnumerable<string> Tenants,
    IEnumerable<string> Roles,
    IEnumerable<string> Groups,
    IEnumerable<Claim> Claims);
