// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Events.Projections;
using Concepts.Applications;
using Events.Applications;

namespace Read.Applications
{
    [Route("/api/applications")]
    public class Applications : Controller
    {
        readonly IMongoCollection<Application> _collection;

        public Applications(IMongoCollection<Application> collection) => _collection = collection;

        [HttpGet]
        public Task<ClientObservable<IEnumerable<Application>>> AllApplications() => _collection.Observe();
    }
}
