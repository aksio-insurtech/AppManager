// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Common;
using Concepts.Azure;
using Concepts.ElasticSearch;
using Concepts.MongoDB;
using Concepts.Pulumi;

namespace Read.Organizations;

public record Settings(
    IEnumerable<AzureSubscription> AzureSubscriptions,
    PulumiOrganization PulumiOrganization,
    PulumiAccessToken PulumiAccessToken,
    MongoDBOrganizationId MongoDBOrganizationId,
    MongoDBPublicKey MongoDBPublicKey,
    MongoDBPrivateKey MongoDBPrivateKey,
    ElasticUrl ElasticUrl,
    ElasticApiKey ElasticApiKey) : ISettings
{
    public static readonly Settings NoSettings = new(
        Array.Empty<AzureSubscription>(),
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty);
}
