// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Organizations;
using ExecutionContext = Aksio.Cratis.Execution.ExecutionContext;

namespace Read.Organizations
{
    [Route("/api/organizations")]
    public class Organizations : Controller
    {
        readonly IMongoCollection<Organization> _collection;

        public Organizations(IMongoCollection<Organization> collection) => _collection = collection;

        [HttpGet]
        public Task<ClientObservable<IEnumerable<Organization>>> AllOrganizations() => _collection.Observe();
    }
}
