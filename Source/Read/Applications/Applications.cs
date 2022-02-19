// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Read.Applications
{
    [Route("/api/applications")]
    public class Applications : Controller
    {
        readonly IMongoCollection<Application> _applicationCollection;
        readonly IMongoCollection<ApplicationResources> _applicationResourcesCollection;
        readonly IMongoCollection<ApplicationsHierarchyForListing> _hierarchyCollection;

        public Applications(
            IMongoCollection<Application> applicationCollection,
            IMongoCollection<ApplicationResources> applicationResourcesCollection,
            IMongoCollection<ApplicationsHierarchyForListing> hierarchyCollection)
        {
            _applicationCollection = applicationCollection;
            _applicationResourcesCollection = applicationResourcesCollection;
            _hierarchyCollection = hierarchyCollection;
        }

        [HttpGet("{applicationId}")]
        public Task<Application> GetApplication([FromRoute] ApplicationId applicationId) => _applicationCollection.FindById(applicationId).FirstOrDefaultAsync();

        [HttpGet("resources/{applicationId}")]
        public Task<ApplicationResources> ResourcesForApplication([FromRoute] ApplicationId applicationId) => _applicationResourcesCollection.FindById(applicationId).FirstOrDefaultAsync();

        [HttpGet("hierarchy")]
        public Task<ClientObservable<IEnumerable<ApplicationsHierarchyForListing>>> ApplicationsHierarchy() => _hierarchyCollection.Observe();
    }
}
