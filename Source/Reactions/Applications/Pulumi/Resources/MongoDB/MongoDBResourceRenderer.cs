// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Resources;
using Pulumi.AzureNative.Network;

namespace Reactions.Applications.Pulumi.Resources.MongoDB;

public class MongoDBResourceRenderer : ICanRenderResource<MongoDBConfiguration>
{
    public const string ResourceTypeId = "cdfc2305-094f-41f2-a141-2567e8cdca61";

    public ResourceTypeId TypeId => ResourceTypeId;
    public ResourceLevel Level => ResourceLevel.Environment;
    public IEnumerable<ResourceTypeId> Dependencies => Enumerable.Empty<ResourceTypeId>();

    public async Task Render(RenderContextForResource context, MongoDBConfiguration configuration)
    {
        var virtualNetwork = context.Results.GetById<VirtualNetwork>(WellKnownResourceTypes.VirtualNetwork);

        var mongoDB = await context.Application.SetupMongoDB(context.Settings, context.ResourceGroup, virtualNetwork, context.Environment, context.Tags);
        context.Results.Register(ResourceTypeId, mongoDB);
    }
}
