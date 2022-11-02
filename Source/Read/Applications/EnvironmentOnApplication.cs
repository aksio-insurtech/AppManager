// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Read.Applications;

public record EnvironmentOnApplication(
    ApplicationEnvironmentId EnvironmentId,
    ApplicationEnvironmentName Name,
    DateTimeOffset LastUpdated,
    DateTimeOffset LastConsolidation,
    IEnumerable<TenantInEnvironment> Tenants,
    IEnumerable<IngressInEnvironment> Ingresses,
    IEnumerable<MicroserviceInEnvironment> Microservices)
{
    public bool HasChanges => LastConsolidation < LastUpdated;
}
