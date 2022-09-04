// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MongoDB.Bson;
using Pulumi.Automation;

namespace Infrastructure;

public class Stacks : IStacks
{
    readonly IMongoCollection<StackDeploymentForApplication> _collection;

    public Stacks(IMongoCollection<StackDeploymentForApplication> collection)
    {
        _collection = collection;
    }

    public async Task<bool> HasFor(ApplicationId applicationId)
    {
        var count = await _collection.CountDocumentsAsync(_ => _.Id == applicationId);
        return count == 1;
    }

    public async Task<StackDeployment> GetFor(ApplicationId applicationId)
    {
        var result = await _collection.FindAsync(_ => _.Id == applicationId);
        var document = result.First();
        return StackDeployment.FromJsonString(document.Deployment.ToJson());
    }

    public async Task Save(ApplicationId applicationId, StackDeployment stackDeployment)
    {
        var stackDeploymentBson = BsonDocument.Parse(stackDeployment.Json.ToJson());
        var document = new StackDeploymentForApplication(applicationId, stackDeploymentBson);
        await _collection.ReplaceOneAsync(_ => _.Id == applicationId, document, new ReplaceOptions { IsUpsert = true });
    }
}
