// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Read.Applications.Environments.Microservices.Deployables;

[Route("/api/applications/{applicationId}/environments/{environmentId}/microservices/{microserviceId}/deployables")]
public class Deployables : Controller
{
    readonly IMongoCollection<Deployable> _deployableCollection;
    readonly IMongoCollection<ConfigFilesForDeployable> _configFilesForDeployableCollection;
    readonly IMongoCollection<EnvironmentVariablesForDeployable> _environmentVariablesForDeployableCollection;
    readonly IMongoCollection<SecretsForDeployable> _secretsForDeployableCollection;

    public Deployables(
        IMongoCollection<Deployable> deployableCollection,
        IMongoCollection<ConfigFilesForDeployable> configFilesForDeployableCollection,
        IMongoCollection<EnvironmentVariablesForDeployable> environmentVariablesForDeployableCollection,
        IMongoCollection<SecretsForDeployable> secretsForDeployableCollection)
    {
        _deployableCollection = deployableCollection;
        _configFilesForDeployableCollection = configFilesForDeployableCollection;
        _environmentVariablesForDeployableCollection = environmentVariablesForDeployableCollection;
        _secretsForDeployableCollection = secretsForDeployableCollection;
    }

    [HttpGet]
    public Task<ClientObservable<IEnumerable<Deployable>>> DeployablesForMicroservice(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId) =>
        _deployableCollection.Observe(_ =>
            _.Id.EnvironmentId == environmentId &&
            _.Id.MicroserviceId == microserviceId);

    [HttpGet("{deployableId}/config-files")]
    public Task<ClientObservable<ConfigFilesForDeployable>> ConfigFilesForDeployableId(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] DeployableId deployableId) =>
        _configFilesForDeployableCollection.ObserveSingle(_ =>
            _.Id.EnvironmentId == environmentId &&
            _.Id.DeployableId == deployableId);

    [HttpGet("{deployableId}/environment-variables")]
    public Task<ClientObservable<EnvironmentVariablesForDeployable>> EnvironmentVariablesForDeployableId(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] DeployableId deployableId) =>
        _environmentVariablesForDeployableCollection.ObserveSingle(_ =>
            _.Id.EnvironmentId == environmentId &&
            _.Id.DeployableId == deployableId);

    [HttpGet("{deployableId}/secrets")]
    public Task<ClientObservable<SecretsForDeployable>> SecretsForDeployableId(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId,
        [FromRoute] DeployableId deployableId) =>
        _secretsForDeployableCollection.ObserveSingle(_ =>
            _.Id.EnvironmentId == environmentId &&
            _.Id.DeployableId == deployableId);
}
