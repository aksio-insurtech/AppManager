// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Read.Applications.Environments.Microservices;

[Route("/api/applications/{applicationId}/environments/{environmentId}/microservices")]
public class Microservices : Controller
{
    readonly IMongoCollection<Microservice> _microserviceCollection;
    readonly IMongoCollection<EnvironmentVariablesForMicroservice> _environmentVariablesForMicroserviceCollection;

    public Microservices(
        IMongoCollection<Microservice> microserviceCollection,
        IMongoCollection<EnvironmentVariablesForMicroservice> environmentVariablesForMicroserviceCollection)
    {
        _microserviceCollection = microserviceCollection;
        _environmentVariablesForMicroserviceCollection = environmentVariablesForMicroserviceCollection;
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

    [HttpGet("{microserviceId}/environment-variables")]
    public Task<ClientObservable<EnvironmentVariablesForMicroservice>> EnvironmentVariablesForMicroserviceId([FromRoute] MicroserviceId microserviceId) =>
        _environmentVariablesForMicroserviceCollection.ObserveById(microserviceId);
}
