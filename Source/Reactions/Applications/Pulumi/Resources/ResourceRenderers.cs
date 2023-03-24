// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Aksio.Cratis.Execution;
using Aksio.Cratis.Types;
using Concepts.Resources;

namespace Reactions.Applications.Pulumi.Resources;

[Singleton]
public class ResourceRenderers : IResourceRenderers
{
    readonly IDictionary<ResourceTypeId, ICanRenderResource> _renderers;
    readonly IDictionary<ResourceTypeId, MethodInfo> _rendererMethods;

    public ResourceRenderers(ITypes types, IServiceProvider serviceProvider)
    {
        var renderers = types
            .FindMultiple(typeof(ICanRenderResource<>))
            .Select(_ => (serviceProvider.GetService(_) as ICanRenderResource)!);

        _rendererMethods = renderers.ToDictionary(_ => _.TypeId, _ => _.GetType().GetMethod(nameof(ICanRenderResource<IResourceConfiguration>.Render), BindingFlags.Instance | BindingFlags.Public)!);
        _renderers = renderers.ToDictionary(_ => _.TypeId, _ => _);
    }

    public async Task Render(ResourceLevel level, RenderContext context, IEnumerable<Resource> resources)
    {
        foreach (var resource in resources)
        {
            if (_renderers.ContainsKey(resource.Type))
            {
                var renderer = _renderers[resource.Type];
                if (renderer.Level != level) continue;

                var renderMethod = _rendererMethods[resource.Type];

                var resourceContext = new RenderContextForResource(
                    resource.Id,
                    resource.Name,
                    context.Application,
                    context.Environment,
                    context.ResourceGroup,
                    context.Tags,
                    context.Results,
                    context.Tenants,
                    context.Microservices);
                await (renderMethod.Invoke(renderer, new object[] { resourceContext, resource.Configuration }) as Task)!;
            }
        }
    }
}
