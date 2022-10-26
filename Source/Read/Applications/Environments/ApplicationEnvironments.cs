// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Read.Applications.Environments;

[Route("/api/applications/{applicationId}/environments")]
public class ApplicationEnvironments : Controller
{
    readonly IMongoCollection<ApplicationEnvironment> _collection;
    readonly IMongoCollection<ApplicationEnvironmentResources> _applicationEnvironmentResourcesCollection;
    readonly IMongoCollection<CustomDomainsForApplicationEnvironment> _domainsCollection;

    public ApplicationEnvironments(
        IMongoCollection<ApplicationEnvironment> collection,
        IMongoCollection<ApplicationEnvironmentResources> applicationEnvironmentResourcesCollection,
        IMongoCollection<CustomDomainsForApplicationEnvironment> domainsCollection)
    {
        _collection = collection;
        _applicationEnvironmentResourcesCollection = applicationEnvironmentResourcesCollection;
        _domainsCollection = domainsCollection;
    }

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

    [HttpGet("{environmentId}/resources")]
    public Task<ApplicationEnvironmentResources> ResourcesForApplication([FromRoute] ApplicationId applicationId) => _applicationEnvironmentResourcesCollection.FindById(applicationId).FirstOrDefaultAsync();

    [HttpGet("environments-for-application")]
    public Task<ClientObservable<IEnumerable<ApplicationEnvironment>>> EnvironmentsForApplication([FromRoute] ApplicationId applicationId) =>
        _collection.Observe(Filters.StringFilterFor<ApplicationEnvironment>(_ => _.Id.ApplicationId, applicationId));

    [HttpGet("{environmentId}/custom-domains")]
    public Task<ClientObservable<CustomDomainsForApplicationEnvironment>> CustomDomainConfigurationForApplicationEnvironment(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId) =>
        _domainsCollection.ObserveId(environmentId);

    [HttpGet("{environmentId}/custom-domains/http")]
    public CustomDomainsForApplicationEnvironment Blah(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId) =>
        _domainsCollection.Find(Filters.StringFilterFor<CustomDomainsForApplicationEnvironment>(_ => _.Id, environmentId)).SingleOrDefault();
}
