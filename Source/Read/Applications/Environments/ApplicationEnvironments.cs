// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Read.Applications.Environments;

[Route("/api/applications/{applicationId}/environments")]
public class ApplicationEnvironments : Controller
{
    readonly IMongoCollection<ApplicationEnvironment> _collection;
    readonly IMongoCollection<ApplicationEnvironmentResources> _applicationEnvironmentResourcesCollection;
    readonly IMongoCollection<EnvironmentVariablesForApplicationEnvironment> _environmentVariablesForApplicationEnvironmentCollection;
    readonly IMongoCollection<CertificatesForApplicationEnvironment> _certificatesForApplicationEnvironmentCollection;

    public ApplicationEnvironments(
        IMongoCollection<ApplicationEnvironment> collection,
        IMongoCollection<ApplicationEnvironmentResources> applicationEnvironmentResourcesCollection,
        IMongoCollection<EnvironmentVariablesForApplicationEnvironment> environmentVariablesForApplicationEnvironmentCollection,
        IMongoCollection<CertificatesForApplicationEnvironment> certificatesForApplicationEnvironmentCollection)
    {
        _collection = collection;
        _applicationEnvironmentResourcesCollection = applicationEnvironmentResourcesCollection;
        _environmentVariablesForApplicationEnvironmentCollection = environmentVariablesForApplicationEnvironmentCollection;
        _certificatesForApplicationEnvironmentCollection = certificatesForApplicationEnvironmentCollection;
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

    [HttpGet("{environmentId}/environment-variables")]
    public Task<ClientObservable<EnvironmentVariablesForApplicationEnvironment>> EnvironmentVariablesForApplicationEnvironmentId([FromRoute] ApplicationEnvironmentId environmentId) =>
        _environmentVariablesForApplicationEnvironmentCollection.ObserveId(environmentId);

    [HttpGet("{environmentId}/certificates")]
    public Task<ClientObservable<CertificatesForApplicationEnvironment>> CertificatesForApplicationEnvironmentId([FromRoute] ApplicationEnvironmentId environmentId) =>
        _certificatesForApplicationEnvironmentCollection.ObserveId(environmentId);
}
