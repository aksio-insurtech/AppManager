// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Resources;
using Pulumi.AzureNative.ServiceBus;
using Pulumi.AzureNative.ServiceBus.Inputs;

namespace Reactions.Applications.Pulumi.Resources;

public class ServiceBusResourceRenderer : ICanRenderResource<ServiceBusConfiguration>
{
    public ResourceTypeId TypeId => "ad902744-ac13-4149-a0a8-cb728cf1d681";

    public Task Render(RenderContextForResource context, ServiceBusConfiguration configuration)
    {
        _ = new global::Pulumi.AzureNative.ServiceBus.Namespace(context.Name, new()
        {
            Location = context.Environment.CloudLocation.Value,
            ResourceGroupName = context.ResourceGroup.Name,
            Sku = new SBSkuArgs
            {
                Name = SkuName.Standard,
                Tier = SkuTier.Standard
            },
            Tags = context.Tags
        });

        return Task.CompletedTask;
    }
}
