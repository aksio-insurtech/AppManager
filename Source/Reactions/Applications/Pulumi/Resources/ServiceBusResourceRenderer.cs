// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Resources;

namespace Reactions.Applications.Pulumi.Resources;

public class ServiceBusResourceRenderer : ICanRenderResource<ServiceBusConfiguration>
{
    public ResourceTypeId TypeId => "ad902744-ac13-4149-a0a8-cb728cf1d681";

    public Task Render(RenderContext context, ServiceBusConfiguration configuration)
    {
        return Task.CompletedTask;
    }
}
