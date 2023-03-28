// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Read.Applications.Environments;

[Route("/api/applications/{applicationId}/environments")]
public class ApplicationEnvironments : Controller
{
    readonly IMongoCollection<ApplicationEnvironment> _applicationEnvironmentCollection;
    readonly IMongoCollection<ConfigFilesForApplicationEnvironment> _configFilesForApplicationEnvironmentCollection;
    readonly IMongoCollection<EnvironmentVariablesForApplicationEnvironment> _environmentVariablesForApplicationEnvironmentCollection;
    readonly IMongoCollection<CertificatesForApplicationEnvironment> _certificatesForApplicationEnvironmentCollection;
    readonly IMongoCollection<SecretsForApplicationEnvironment> _secretsForApplicationEnvironmentCollection;

    public ApplicationEnvironments(
        IMongoCollection<ApplicationEnvironment> collection,
        IMongoCollection<ConfigFilesForApplicationEnvironment> configFilesForApplicationEnvironmentCollection,
        IMongoCollection<EnvironmentVariablesForApplicationEnvironment> environmentVariablesForApplicationEnvironmentCollection,
        IMongoCollection<CertificatesForApplicationEnvironment> certificatesForApplicationEnvironmentCollection,
        IMongoCollection<SecretsForApplicationEnvironment> secretsForApplicationEnvironmentCollection)
    {
        _applicationEnvironmentCollection = collection;
        _configFilesForApplicationEnvironmentCollection = configFilesForApplicationEnvironmentCollection;
        _environmentVariablesForApplicationEnvironmentCollection = environmentVariablesForApplicationEnvironmentCollection;
        _certificatesForApplicationEnvironmentCollection = certificatesForApplicationEnvironmentCollection;
        _secretsForApplicationEnvironmentCollection = secretsForApplicationEnvironmentCollection;
    }

    [HttpGet("{environmentId}")]
    public async Task<ApplicationEnvironment> EnvironmentForApplication(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId)
    {
        var result = await _applicationEnvironmentCollection.FindAsync(_ => _.Id.ApplicationId == applicationId && _.Id.EnvironmentId == environmentId);
        return result.FirstOrDefault();
    }

    [HttpGet("environments-for-application")]
    public Task<ClientObservable<IEnumerable<ApplicationEnvironment>>> EnvironmentsForApplication([FromRoute] ApplicationId applicationId) =>
        _applicationEnvironmentCollection.Observe(_ => _.Id.ApplicationId == applicationId);

    [HttpGet("{environmentId}/config-files")]
    public Task<ClientObservable<ConfigFilesForApplicationEnvironment>> ConfigFilesForApplicationEnvironmentId([FromRoute] ApplicationEnvironmentId environmentId) =>
        _configFilesForApplicationEnvironmentCollection.ObserveById(environmentId);

    [HttpGet("{environmentId}/environment-variables")]
    public Task<ClientObservable<EnvironmentVariablesForApplicationEnvironment>> EnvironmentVariablesForApplicationEnvironmentId([FromRoute] ApplicationEnvironmentId environmentId) =>
        _environmentVariablesForApplicationEnvironmentCollection.ObserveById(environmentId);

    [HttpGet("{environmentId}/secrets")]
    public Task<ClientObservable<SecretsForApplicationEnvironment>> SecretsForApplicationEnvironmentId([FromRoute] ApplicationEnvironmentId environmentId) =>
        _secretsForApplicationEnvironmentCollection.ObserveById(environmentId);

    [HttpGet("{environmentId}/certificates")]
    public Task<ClientObservable<CertificatesForApplicationEnvironment>> CertificatesForApplicationEnvironmentId([FromRoute] ApplicationEnvironmentId environmentId) =>
        _certificatesForApplicationEnvironmentCollection.ObserveById(environmentId);
}
