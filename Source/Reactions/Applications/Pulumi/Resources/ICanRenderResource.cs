// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Resources;

namespace Reactions.Applications.Pulumi.Resources;

public interface ICanRenderResource
{
    ResourceLevel Level { get; }
    ResourceTypeId TypeId { get; }
    IEnumerable<ResourceTypeId> Dependencies { get; }
}

public interface ICanRenderResource<TConfiguration> : ICanRenderResource
    where TConfiguration : IResourceConfiguration
{
    Task Render(RenderContextForResource context, TConfiguration configuration);
}
