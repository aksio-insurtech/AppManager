// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Events.Projections;
using Concepts.Applications;
using Events.Applications;
using MongoDB.Bson;

namespace Read.Applications
{
    [Route("/api/applications")]
    public class Applications : Controller
    {
        readonly IMongoCollection<Application> _applicationCollection;
        readonly IMongoCollection<ApplicationsHierarchyForListing> _hierarchyCollection;

        public Applications(
            IMongoCollection<Application> applicationCollection,
            IMongoCollection<ApplicationsHierarchyForListing> hierarchyCollection)
        {
            _applicationCollection = applicationCollection;
            _hierarchyCollection = hierarchyCollection;
        }

        [HttpGet("{applicationId}")]
        public Task<Application> GetApplication([FromRoute] ApplicationId applicationId) => _applicationCollection.FindById(applicationId).FirstOrDefaultAsync();

        [HttpGet("hierarchy")]
        public Task<ClientObservable<IEnumerable<ApplicationsHierarchyForListing>>> ApplicationsHierarchy() => _hierarchyCollection.Observe();
    }
}
