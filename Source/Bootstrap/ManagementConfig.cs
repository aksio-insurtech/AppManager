// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Bootstrap;

public record ManagementConfig(
    Guid TenantId,
    string OrganizationName,
    string Name,
    CloudLocationKey CloudLocation,
    AzureConfig Azure,
    PulumiConfig Pulumi,
    MongoDBConfig MongoDB,
    AuthenticationConfig Authentication);
