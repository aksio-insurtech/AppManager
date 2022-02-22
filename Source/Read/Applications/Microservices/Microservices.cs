// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Read.Applications.Microservices
{
    [Route("/api/applications/{applicationId}/microservices")]
    public class Microservices : Controller
    {
        readonly IMongoCollection<Microservice> _microserviceCollection;

        public Microservices(IMongoCollection<Microservice> microserviceCollection)
        {
            _microserviceCollection = microserviceCollection;
        }

        [HttpGet("{microserviceId}")]
        public Task<Microservice> GetMicroservice([FromRoute] MicroserviceId microserviceId) => _microserviceCollection.FindById(microserviceId).FirstOrDefaultAsync();
    }
}
