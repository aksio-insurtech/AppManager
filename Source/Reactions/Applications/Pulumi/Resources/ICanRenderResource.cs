// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Resources;

namespace Reactions.Applications.Pulumi.Resources;

public interface ICanRenderResource
{
    ResourceTypeId TypeId { get; }
}

public interface ICanRenderResource<TConfiguration> : ICanRenderResource
    where TConfiguration : IResourceConfiguration
{
    Task Render(RenderContext context, TConfiguration configuration);
}
