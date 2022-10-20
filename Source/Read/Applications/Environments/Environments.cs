// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Read.Applications.Environments;

[Route("/api/applications/{applicationId}/environments")]
public class Environments : Controller
{
    readonly IMongoCollection<ApplicationEnvironment> _collection;

    public Environments(IMongoCollection<ApplicationEnvironment> collection) => _collection = collection;

    [HttpGet("environments-for-application")]
    public Task<ClientObservable<IEnumerable<ApplicationEnvironment>>> EnvironmentsForApplication([FromRoute] ApplicationId applicationId) =>
        _collection.Observe(Filters.StringFilterFor<ApplicationEnvironment>(_ => _.ApplicationId, applicationId));
}
