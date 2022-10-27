// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Concepts.Applications.Environments.Ingresses.IdentityProviders;

namespace Events.Applications.Environments.Ingresses.IdentityProviders;

[EventType("1954ca0d-b113-4f15-a9bf-8d645b615275")]
public record IdentityProviderAuthUrlSet(IdentityProviderId IdentityProviderId, Url Url);
