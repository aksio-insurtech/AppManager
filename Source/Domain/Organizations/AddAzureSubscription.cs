// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Organizations;
using Events.Organizations;

namespace Domain.Organizations
{
    public record AddAzureSubscription(AzureSubscriptionId Id, AzureSubscriptionName Name);
}
