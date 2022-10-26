// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Azure;

namespace Events.Applications.Environments;

[EventType("3494e0df-619a-4b5b-bdb3-f668586ac898")]
public record AzureResourceGroupCreatedForApplication(AzureSubscriptionId SubscriptionId, AzureResourceGroupId ResourceGroupId);
