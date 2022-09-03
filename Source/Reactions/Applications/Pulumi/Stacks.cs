// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MongoDB.Bson;
using Pulumi.Automation;

namespace Reactions.Applications.Pulumi;

public class Stacks : IStacks
{
    readonly IMongoCollection<StackDeploymentForApplication> _collection;

    public Stacks(IMongoCollection<StackDeploymentForApplication> collection)
    {
        _collection = collection;
    }

    public async Task<bool> HasFor(Application application)
    {
        var count = await _collection.CountDocumentsAsync(_ => _.Id == application.Id);
        return count == 1;
    }

    public async Task<StackDeployment> GetFor(Application application)
    {
        var result = await _collection.FindAsync(_ => _.Id == application.Id);
        var document = result.First();
        return StackDeployment.FromJsonString(document.Deployment.ToJson());
    }

    public async Task Save(Application application, StackDeployment stackDeployment)
    {
        var stackDeploymentBson = BsonDocument.Parse(stackDeployment.Json.ToJson());
        var document = new StackDeploymentForApplication(application.Id, stackDeploymentBson);
        await _collection.ReplaceOneAsync(_ => _.Id == application.Id, document, new ReplaceOptions { IsUpsert = true });
    }
}
