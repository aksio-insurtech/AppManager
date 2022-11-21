// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Read.Applications.Environments.Microservices.Deployables;

[Route("/api/applications/{applicationId}/environments/{environmentId}/microservices/{microserviceId}/deployables")]
public class Deployables : Controller
{
    readonly IMongoCollection<Deployable> _deployableCollection;
    readonly IMongoCollection<EnvironmentVariablesForDeployable> _environmentVariablesForDeployableCollection;

    public Deployables(
        IMongoCollection<Deployable> deployableCollection,
        IMongoCollection<EnvironmentVariablesForDeployable> environmentVariablesForDeployableCollection)
    {
        _deployableCollection = deployableCollection;
        _environmentVariablesForDeployableCollection = environmentVariablesForDeployableCollection;
    }

    [HttpGet]
    public Task<ClientObservable<IEnumerable<Deployable>>> DeployablesForMicroservice(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId) =>
            _deployableCollection.Observe(_ =>
                _.Id.EnvironmentId == environmentId &&
                _.Id.MicroserviceId == microserviceId);

    [HttpGet("{deployableId}/environment-variables")]
    public Task<ClientObservable<EnvironmentVariablesForDeployable>> EnvironmentVariablesForDeployableId([FromRoute] DeployableId deployableId) =>
        _environmentVariablesForDeployableCollection.ObserveById(deployableId);
}
