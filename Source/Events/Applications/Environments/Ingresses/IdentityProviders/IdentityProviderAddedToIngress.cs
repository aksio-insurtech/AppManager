// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments.Ingresses.IdentityProviders;

namespace Events.Applications.Environments.Ingresses.IdentityProviders;

[EventType("9f638879-6f86-493e-ae82-2dbb14a73373")]
public record IdentityProviderAddedToIngress(IdentityProviderId IdentityProviderId, IdentityProviderName Name, IdentityProviderType Type);
