// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Resources;

namespace Reactions.Applications.Pulumi.Resources;

public class ResourceRenderingScope
{
    readonly List<Resource> _renderedResources = new();

    public IEnumerable<Resource> Resources { get; }

    public IEnumerable<Resource> RenderedResources => _renderedResources;

    public ResourceRenderingScope(IEnumerable<Resource> resources)
    {
        Resources = resources;
    }

    public void Rendered(Resource resource)
    {
        _renderedResources.Add(resource);
    }

    public bool HasRendered(ResourceTypeId resourceTypeId) =>
        _renderedResources.Any(_ => _.Type == resourceTypeId);

    public Resource GetResource(ResourceTypeId resourceTypeId) =>
        (_renderedResources.SingleOrDefault(_ => _.Type == resourceTypeId) ??
            Resources.SingleOrDefault(_ => _.Type == resourceTypeId)) ??
            throw new ResourceNotFound(resourceTypeId);
}
