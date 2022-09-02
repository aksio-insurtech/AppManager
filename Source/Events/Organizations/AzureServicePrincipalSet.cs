// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Azure;

namespace Events.Organizations;

[EventType("9ac3cf84-ef63-4b57-bec7-730b2975a993")]
public record AzureServicePrincipalSet(AzureServicePrincipalClientId ClientId, AzureServicePrincipalClientSecret ClientSecret);
