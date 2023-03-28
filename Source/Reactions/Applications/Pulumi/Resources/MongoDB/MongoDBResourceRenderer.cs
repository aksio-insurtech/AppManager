// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Concepts.Resources;
using Events.Applications.Environments;
using Pulumi.AzureNative.Network;

namespace Reactions.Applications.Pulumi.Resources.MongoDB;

public class MongoDBResourceRenderer : ICanRenderResource<MongoDBConfiguration>
{
    public const string ResourceTypeId = "cdfc2305-094f-41f2-a141-2567e8cdca61";
    readonly IExecutionContextManager _executionContextManager;
    readonly IEventLog _eventLog;

    public ResourceTypeId TypeId => ResourceTypeId;
    public ResourceLevel Level => ResourceLevel.Environment;
    public IEnumerable<ResourceTypeId> Dependencies => Enumerable.Empty<ResourceTypeId>();

    public MongoDBResourceRenderer(IExecutionContextManager executionContextManager, IEventLog eventLog)
    {
        _executionContextManager = executionContextManager;
        _eventLog = eventLog;
    }

    public async Task Render(RenderContextForResource context, MongoDBConfiguration configuration)
    {
        var virtualNetwork = context.Results.GetById<VirtualNetwork>(WellKnownResourceTypes.VirtualNetwork);

        var mongoDB = await context.Application.SetupMongoDB(context.Settings, context.ResourceGroup, virtualNetwork, context.Environment, context.Tags);
        context.Results.Register(ResourceTypeId, mongoDB);

        _executionContextManager.Set(context.ExecutionContext);
        if (mongoDB.ConnectionString != context.Environment.MongoDB.ConnectionString)
        {
            await _eventLog.Append(context.Environment.Id, new MongoDBConnectionStringChangedForApplicationEnvironment(mongoDB.ConnectionString));
        }

        var kernelUser = context.Environment.MongoDB?.Users.FirstOrDefault(_ => _.UserName == "kernel");
        if (mongoDB.Password != kernelUser?.Password)
        {
            await _eventLog.Append(context.Environment.Id, new MongoDBUserChanged("kernel", mongoDB.Password));
        }
    }
}
