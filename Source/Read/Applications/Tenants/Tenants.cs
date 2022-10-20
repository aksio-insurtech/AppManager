// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Read.Applications.Environments.Tenants;

[Route("/api/applications/{applicationId}/environments/{environmentId}/tenants")]
public class Tenants : Controller
{
    readonly IMongoCollection<Tenant> _collection;

    public Tenants(IMongoCollection<Tenant> collection) => _collection = collection;

    [HttpGet]
    public Task<ClientObservable<IEnumerable<Tenant>>> TenantsForEnvironment([FromRoute] ApplicationEnvironmentId environmentId) =>
        _collection.Observe(Filters.StringFilterFor<Tenant>(_ => _.EnvironmentId, environmentId));
}
