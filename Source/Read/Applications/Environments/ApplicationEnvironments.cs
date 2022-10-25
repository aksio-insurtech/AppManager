// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Read.Applications.Environments;

[Route("/api/applications/{applicationId}/environments")]
public class ApplicationEnvironments : Controller
{
    readonly IMongoCollection<ApplicationEnvironment> _collection;

    public ApplicationEnvironments(IMongoCollection<ApplicationEnvironment> collection) => _collection = collection;

    [HttpGet("{environmentId}")]
    public async Task<ApplicationEnvironment> EnvironmentForApplication(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId)
    {
        var result = await _collection.FindAsync(Builders<ApplicationEnvironment>.Filter.And(
            Filters.StringFilterFor<ApplicationEnvironment>(_ => _.Id.ApplicationId, applicationId),
            Filters.StringFilterFor<ApplicationEnvironment>(_ => _.Id.EnvironmentId, environmentId)));
        return result.FirstOrDefault();
    }

    [HttpGet("environments-for-application")]
    public Task<ClientObservable<IEnumerable<ApplicationEnvironment>>> EnvironmentsForApplication([FromRoute] ApplicationId applicationId) =>
        _collection.Observe(Filters.StringFilterFor<ApplicationEnvironment>(_ => _.Id.ApplicationId, applicationId));
}
