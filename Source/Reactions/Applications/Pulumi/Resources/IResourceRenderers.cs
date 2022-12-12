// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions.Applications.Pulumi.Resources;

public interface IResourceRenderers
{
    Task Render(RenderContext context, IEnumerable<Resource> resources);
}
