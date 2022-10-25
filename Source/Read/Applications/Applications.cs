// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Read.Applications;

[Route("/api/applications")]
public class Applications : Controller
{
    readonly IMongoCollection<Application> _applicationCollection;
    readonly IMongoCollection<ApplicationResources> _applicationResourcesCollection;
    readonly IMongoCollection<ApplicationHierarchyForListing> _hierarchyCollection;

    public Applications(
        IMongoCollection<Application> applicationCollection,
        IMongoCollection<ApplicationResources> applicationResourcesCollection,
        IMongoCollection<ApplicationHierarchyForListing> hierarchyCollection)
    {
        _applicationCollection = applicationCollection;
        _applicationResourcesCollection = applicationResourcesCollection;
        _hierarchyCollection = hierarchyCollection;
    }

    [HttpGet("{applicationId}")]
    public Task<Application> GetApplication([FromRoute] ApplicationId applicationId) => _applicationCollection.FindById(applicationId).FirstOrDefaultAsync();

    [HttpGet("{applicationId}/resources")]
    public Task<ApplicationResources> ResourcesForApplication([FromRoute] ApplicationId applicationId) => _applicationResourcesCollection.FindById(applicationId).FirstOrDefaultAsync();

    [HttpGet("hierarchy")]
    public Task<ClientObservable<IEnumerable<ApplicationHierarchyForListing>>> ApplicationsHierarchy() => _hierarchyCollection.Observe();
}
