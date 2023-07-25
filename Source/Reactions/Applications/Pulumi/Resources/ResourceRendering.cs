// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Aksio.Execution;
using Aksio.Types;
using Concepts.Resources;

namespace Reactions.Applications.Pulumi.Resources;

[Singleton]
public class ResourceRendering : IResourceRendering
{
    readonly IDictionary<ResourceTypeId, ICanRenderResource> _renderers;
    readonly IDictionary<ResourceTypeId, MethodInfo> _rendererMethods;

    public ResourceRendering(ITypes types, IServiceProvider serviceProvider)
    {
        var renderers = types
            .FindMultiple(typeof(ICanRenderResource<>))
            .Select(_ => (serviceProvider.GetService(_) as ICanRenderResource)!);

        _rendererMethods = renderers.ToDictionary(_ => _.TypeId, _ => _.GetType().GetMethod(nameof(ICanRenderResource<IResourceConfiguration>.Render), BindingFlags.Instance | BindingFlags.Public)!);
        _renderers = renderers.ToDictionary(_ => _.TypeId, _ => _);
    }

    public async Task Render(ResourceLevel level, RenderContext context, ResourceRenderingScope scope)
    {
        foreach (var resource in scope.Resources)
        {
            await RenderResource(level, context, resource, scope);
        }
    }

    async Task RenderResource(ResourceLevel level, RenderContext context, Resource resource, ResourceRenderingScope scope)
    {
        if (_renderers.ContainsKey(resource.Type))
        {
            var renderer = _renderers[resource.Type];
            if (renderer.Level != level) return;

            if (scope.HasRendered(resource.Type))
            {
                return;
            }

            foreach (var dependency in renderer.Dependencies)
            {
                if (!scope.HasRendered(dependency))
                {
                    var dependencyResource = scope.GetResource(dependency);
                    await RenderResource(level, context, dependencyResource, scope);
                }
            }

            var renderMethod = _rendererMethods[resource.Type];

            var resourceContext = new RenderContextForResource(
                context.ExecutionContext,
                context.Settings,
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

            scope.Rendered(resource);
        }
    }
}
