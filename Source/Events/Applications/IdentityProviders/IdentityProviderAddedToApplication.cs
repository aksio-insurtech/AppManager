// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.IdentityProviders;

namespace Events.Applications.IdentityProviders;

[EventType("9f638879-6f86-493e-ae82-2dbb14a73373")]
public record IdentityProviderAddedToApplication(IdentityProviderId IdentityProviderId, IdentityProviderName Name, IdentityProviderType Type);
