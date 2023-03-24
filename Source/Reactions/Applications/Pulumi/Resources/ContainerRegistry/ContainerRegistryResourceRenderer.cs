// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Resources;

namespace Reactions.Applications.Pulumi.Resources.ContainerRegistry;

public class ContainerRegistryResourceRenderer : ICanRenderResource<ContainerRegistryConfiguration>
{
    public ResourceTypeId TypeId => "421ff081-42a8-4b23-897f-84464ca16ac3";
    public ResourceLevel Level => ResourceLevel.Shared;
    public IEnumerable<ResourceTypeId> Dependencies => Enumerable.Empty<ResourceTypeId>();

    public Task Render(RenderContextForResource context, ContainerRegistryConfiguration configuration)
    {
        context.Application.SetupContainerRegistry(context.ResourceGroup, context.Tags);
        return Task.CompletedTask;
    }
}
