// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Read.Applications;

[Route("/api/applications")]
public class Applications : Controller
{
    readonly IMongoCollection<Application> _applicationCollection;
    readonly IMongoCollection<ApplicationHierarchyForListing> _hierarchyCollection;
    readonly IMongoCollection<EnvironmentVariablesForApplication> _environmentVariablesForApplicationCollection;
    readonly IMongoCollection<ConfigFilesForApplication> _configFilesForApplicationCollection;
    readonly IMongoCollection<SecretsForApplication> _secretsForApplicationCollection;

    public Applications(
        IMongoCollection<Application> applicationCollection,
        IMongoCollection<ApplicationHierarchyForListing> hierarchyCollection,
        IMongoCollection<EnvironmentVariablesForApplication> environmentVariablesForApplicationCollection,
        IMongoCollection<ConfigFilesForApplication> configFilesForApplicationCollection,
        IMongoCollection<SecretsForApplication> secretsForApplicationCollection)
    {
        _applicationCollection = applicationCollection;
        _hierarchyCollection = hierarchyCollection;
        _environmentVariablesForApplicationCollection = environmentVariablesForApplicationCollection;
        _configFilesForApplicationCollection = configFilesForApplicationCollection;
        _secretsForApplicationCollection = secretsForApplicationCollection;
    }

    [HttpGet("{applicationId}")]
    public Task<Application> GetApplication([FromRoute] ApplicationId applicationId) => _applicationCollection.FindById(applicationId).FirstOrDefaultAsync();

    [HttpGet("hierarchy")]
    public Task<ClientObservable<IEnumerable<ApplicationHierarchyForListing>>> ApplicationsHierarchy() => _hierarchyCollection.Observe();

    [HttpGet("{applicationId}/environment-variables")]
    public Task<ClientObservable<EnvironmentVariablesForApplication>> EnvironmentVariablesForApplicationId([FromRoute] ApplicationId applicationId) =>
        _environmentVariablesForApplicationCollection.ObserveById(applicationId);

    [HttpGet("{applicationId}/config-files")]
    public Task<ClientObservable<ConfigFilesForApplication>> ConfigFilesForApplicationId([FromRoute] ApplicationId applicationId) =>
        _configFilesForApplicationCollection.ObserveById(applicationId);

    [HttpGet("{applicationId}/secrets")]
    public Task<ClientObservable<SecretsForApplication>> SecretsForApplicationId([FromRoute] ApplicationId applicationId) =>
        _secretsForApplicationCollection.ObserveById(applicationId);
}
