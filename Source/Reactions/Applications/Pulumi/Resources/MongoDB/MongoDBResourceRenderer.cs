// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Common;
using Concepts.Resources;
using Pulumi.AzureNative.Network;

namespace Reactions.Applications.Pulumi.Resources.MongoDB;

public class MongoDBResourceRenderer : ICanRenderResource<MongoDBConfiguration>
{
    public const string ResourceTypeId = "421ff081-42a8-4b23-897f-84464ca16ac3";
    readonly ISettings _settings;

    public ResourceTypeId TypeId => ResourceTypeId;
    public ResourceLevel Level => ResourceLevel.Environment;
    public IEnumerable<ResourceTypeId> Dependencies => Enumerable.Empty<ResourceTypeId>();

    public MongoDBResourceRenderer(ISettings settings)
    {
        _settings = settings;
    }

    public async Task Render(RenderContextForResource context, MongoDBConfiguration configuration)
    {
        var virtualNetwork = context.Results.GetById<VirtualNetwork>(WellKnownResourceTypes.VirtualNetwork);

        var mongoDB = await context.Application.SetupMongoDB(_settings, context.ResourceGroup, virtualNetwork, context.Environment, context.Tags);
        context.Results.Register(ResourceTypeId, mongoDB);
    }
}
