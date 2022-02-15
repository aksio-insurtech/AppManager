// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Azure;
using Concepts.ElasticSearch;
using Concepts.MongoDB;
using Concepts.Pulumi;

namespace Common
{
    public interface IApplicationSettings
    {
        IEnumerable<AzureSubscription> AzureSubscriptions { get; }
        PulumiAccessToken PulumiAccessToken { get; }
        MongoDBOrganizationId MongoDBOrganizationId { get; }
        MongoDBPublicKey MongoDBPublicKey { get; }
        MongoDBPrivateKey MongoDBPrivateKey { get; }
        ElasticUrl ElasticUrl { get; }
        ElasticApiKey ElasticApiKey { get; }
    }
}
