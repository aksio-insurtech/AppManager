// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Events.Projections;
using Concepts.Organizations;
using Concepts.Pulumi;
using Events.Organizations;

namespace Read.Organizations
{
    public record Settings(string Id, IEnumerable<AzureSubscription> AzureSubscriptions, PulumiAccessToken PulumiAccessToken)
    {
        public static readonly Settings NoSettings = new(string.Empty, Array.Empty<AzureSubscription>(), string.Empty);
    }
}
