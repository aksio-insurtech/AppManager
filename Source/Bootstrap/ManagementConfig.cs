// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Azure;

namespace Bootstrap;

public record ManagementConfig(
    string Name,
    CloudLocationKey CloudLocation,
    AzureSubscription Azure,
    PulumiConfig Pulumi,
    MongoDBConfig MongoDB,
    ElasticSearchConfig Elasticsearch,
    AuthenticationConfig Authentication);
