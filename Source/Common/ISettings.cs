// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Azure;
using Concepts.MongoDB;
using Concepts.Pulumi;

namespace Common;

public interface ISettings
{
    IEnumerable<AzureSubscription> AzureSubscriptions { get; }
    PulumiOrganization PulumiOrganization { get; }
    PulumiAccessToken PulumiAccessToken { get; }
    MongoDBOrganizationId MongoDBOrganizationId { get; }
    MongoDBPublicKey MongoDBPublicKey { get; }
    MongoDBPrivateKey MongoDBPrivateKey { get; }
    AzureServicePrincipal ServicePrincipal { get; }
}
