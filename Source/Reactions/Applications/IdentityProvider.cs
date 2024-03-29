// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments.Ingresses.IdentityProviders;
using Reactions.Applications.Templates;

namespace Reactions.Applications;

public record IdentityProvider(
    IdentityProviderId Id,
    IdentityProviderName Name,
    IdentityProviderType Type,
    IdentityProviderClientId ClientId,
    IdentityProviderClientSecret ClientSecret,
    IdentityProviderIssuerURL Issuer,
    IdentityProviderAuthorizationEndpoint AuthorizationEndpoint,
    IdentityProviderTokenEndpoint TokenEndpoint,
    IdentityProviderCertificationURL CertificationUri,
    IdentityProviderImpersonation? Impersonation,
    AuthorizationConfig? Authorization);