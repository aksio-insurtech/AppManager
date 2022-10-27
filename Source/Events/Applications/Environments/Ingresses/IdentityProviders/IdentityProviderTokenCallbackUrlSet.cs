// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;

namespace Events.Applications.Environments.Ingresses.IdentityProviders;

[EventType("8854df16-35f3-4b8f-9b92-84f3d5a301ca")]
public record IdentityProviderTokenCallbackUrlSet(Url Url);
