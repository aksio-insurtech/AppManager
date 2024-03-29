// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Azure;

namespace Bootstrap;

public record AzureConfig(
    string TenantId,
    AzureSubscriptionId SharedSubscriptionId,
    AzureSubscriptionId EnvironmentSubscriptionId,
    IEnumerable<AzureSubscription> Subscriptions,
    AzureServicePrincipal ServicePrincipal);
