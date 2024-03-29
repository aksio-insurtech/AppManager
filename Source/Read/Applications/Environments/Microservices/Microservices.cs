// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Read.Applications.Environments.Microservices;

[Route("/api/applications/{applicationId}/environments/{environmentId}/microservices")]
public class Microservices : Controller
{
    readonly IMongoCollection<Microservice> _microserviceCollection;
    readonly IMongoCollection<AppSettingsForMicroservice> _appSettingsForMicroserviceCollection;
    readonly IMongoCollection<ConfigFilesForMicroservice> _configFilesForMicroserviceCollection;
    readonly IMongoCollection<EnvironmentVariablesForMicroservice> _environmentVariablesForMicroserviceCollection;
    readonly IMongoCollection<SecretsForMicroservice> _secretsForMicroserviceCollection;

    public Microservices(
        IMongoCollection<Microservice> microserviceCollection,
        IMongoCollection<AppSettingsForMicroservice> appSettingsForMicroserviceCollection,
        IMongoCollection<ConfigFilesForMicroservice> configFilesForMicroserviceCollection,
        IMongoCollection<EnvironmentVariablesForMicroservice> environmentVariablesForMicroserviceCollection,
        IMongoCollection<SecretsForMicroservice> secretsForMicroserviceCollection)
    {
        _microserviceCollection = microserviceCollection;
        _appSettingsForMicroserviceCollection = appSettingsForMicroserviceCollection;
        _configFilesForMicroserviceCollection = configFilesForMicroserviceCollection;
        _environmentVariablesForMicroserviceCollection = environmentVariablesForMicroserviceCollection;
        _secretsForMicroserviceCollection = secretsForMicroserviceCollection;
    }

    [HttpGet]
    public async Task<IEnumerable<Microservice>> MicroservicesInEnvironment(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId)
    {
        var result = await _microserviceCollection.FindAsync(_ => _.Id.EnvironmentId == environmentId);
        return result.ToEnumerable();
    }

    [HttpGet("{microserviceId}")]
    public Task<Microservice> GetMicroservice(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId) =>
        _microserviceCollection.Find(_ =>
            _.Id.EnvironmentId == environmentId &&
            _.Id.MicroserviceId == microserviceId).FirstOrDefaultAsync();

    [HttpGet("{microserviceId}/app-settings")]
    public Task<ClientObservable<AppSettingsForMicroservice>> AppSettingsForMicroserviceId(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId) =>
        _appSettingsForMicroserviceCollection.ObserveSingle(_ =>
            _.Id.EnvironmentId == environmentId &&
            _.Id.MicroserviceId == microserviceId);

    [HttpGet("{microserviceId}/config-files")]
    public Task<ClientObservable<ConfigFilesForMicroservice>> ConfigFilesForMicroserviceId(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId) =>
        _configFilesForMicroserviceCollection.ObserveSingle(_ =>
            _.Id.EnvironmentId == environmentId &&
            _.Id.MicroserviceId == microserviceId);

    [HttpGet("{microserviceId}/environment-variables")]
    public Task<ClientObservable<EnvironmentVariablesForMicroservice>> EnvironmentVariablesForMicroserviceId(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId) =>
        _environmentVariablesForMicroserviceCollection.ObserveSingle(_ =>
            _.Id.EnvironmentId == environmentId &&
            _.Id.MicroserviceId == microserviceId);

    [HttpGet("{microserviceId}/secrets")]
    public Task<ClientObservable<SecretsForMicroservice>> SecretsForMicroserviceId(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId) =>
        _secretsForMicroserviceCollection.ObserveSingle(_ =>
            _.Id.EnvironmentId == environmentId &&
            _.Id.MicroserviceId == microserviceId);
}
