// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments.Ingresses.IdentityProviders;

namespace Bootstrap;

public record IdentityProviderConfig(
    IdentityProviderId Id,
    IdentityProviderClientId ClientId,
    IdentityProviderClientSecret ClientSecret);
