// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Resources;
using Pulumi;
using Pulumi.AzureNative.ServiceBus;
using Pulumi.AzureNative.ServiceBus.Inputs;

namespace Reactions.Applications.Pulumi.Resources.ServiceBus;

public class ServiceBusResourceRenderer : ICanRenderResource<ServiceBusConfiguration>
{
    public ResourceTypeId TypeId => "ad902744-ac13-4149-a0a8-cb728cf1d681";

    public ResourceLevel Level => ResourceLevel.Environment;
    public IEnumerable<ResourceTypeId> Dependencies => Enumerable.Empty<ResourceTypeId>();

    public async Task Render(RenderContextForResource context, ServiceBusConfiguration configuration)
    {
        var @namespace = new Namespace(context.Name, new()
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

        var namespaceRule = new NamespaceAuthorizationRule("all", new()
        {
            NamespaceName = @namespace.Name,
            ResourceGroupName = context.ResourceGroup.Name,
            Rights = new[]
            {
                AccessRights.Listen,
                AccessRights.Send
            }
        });

        // TODO: Register connection string result
        var namespaceKeys = Output
            .Tuple(@namespace.Name, namespaceRule.Name)
            .Apply(_ => ListNamespaceKeys.InvokeAsync(new()
            {
                NamespaceName = _.Item1,
                AuthorizationRuleName = _.Item2,
                ResourceGroupName = context.ResourceGroup.GetResourceName()
            }));
        var namespaceKeysValue = await namespaceKeys.GetValue();
        Console.WriteLine(namespaceKeysValue.PrimaryConnectionString);
    }
}
