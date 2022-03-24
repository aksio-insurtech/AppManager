// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.IdentityProviders;

namespace Events.Applications.IdentityProviders;

[EventType("9db3cdbd-57b9-48e4-94a2-3132bafb27ed")]
public record IdentityProviderClientSettingsSet(IdentityProviderId IdentityProviderId, IdentityProviderClientId ClientId, IdentityProviderClientSecret ClientSecret);
