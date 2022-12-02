// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments.Ingresses.IdentityProviders;
using Concepts.Applications.Tenants;

namespace Reactions.Applications;

public record TenantIdentityProviderConfig(IdentityProviderId Id, Domain? Domain, OnBehalfOf? OnBehalfOf);
