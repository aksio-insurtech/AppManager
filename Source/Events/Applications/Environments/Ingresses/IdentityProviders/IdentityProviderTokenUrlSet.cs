// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Concepts.Applications.Environments.Ingresses.IdentityProviders;

namespace Events.Applications.Environments.Ingresses.IdentityProviders;

[EventType("1f26d6fb-edbe-43b2-829f-9fa4417ab1ff")]
public record IdentityProviderTokenUrlSet(IdentityProviderId IdentityProviderId, Url Url);
