// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Read.Applications.Environments.Microservices;

[Route("/api/applications/{applicationId}/environments/{environmentId}/microservices")]
public class Microservices : Controller
{
    readonly IMongoCollection<Microservice> _microserviceCollection;

    public Microservices(IMongoCollection<Microservice> microserviceCollection)
    {
        _microserviceCollection = microserviceCollection;
    }

    [HttpGet("{microserviceId}")]
    public Task<Microservice> GetMicroservice(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId) =>
        _microserviceCollection.Find(
            Builders<Microservice>.Filter.And(
                Filters.StringFilterFor<Microservice>(_ => _.Id.ApplicationId, applicationId),
                Filters.StringFilterFor<Microservice>(_ => _.Id.EnvironmentId, environmentId),
                Filters.StringFilterFor<Microservice>(_ => _.Id.MicroserviceId, microserviceId))).FirstOrDefaultAsync();
}
