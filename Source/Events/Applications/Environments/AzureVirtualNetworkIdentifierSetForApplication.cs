// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Azure;

namespace Events.Applications.Environments;

[EventType("7be14bcb-b9d2-4ae1-ae1e-303134eb7269")]
public record AzureVirtualNetworkIdentifierSetForApplication(AzureVirtualNetworkIdentifier Identifier);
