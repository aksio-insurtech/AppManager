// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Azure;

namespace Events.Applications;

[EventType("d9110645-7c86-49a9-8cc8-ed1db66ea28f")]
public record AzureNetworkProfileIdentifierSetForApplication(AzureNetworkProfileIdentifier Identifier);
