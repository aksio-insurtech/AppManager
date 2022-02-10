// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Azure;

namespace Read.Organizations
{
    public record AzureSubscription(AzureSubscriptionId SubscriptionId, AzureSubscriptionName Name);
}
