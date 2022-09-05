// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Pulumi.Automation;

namespace Infrastructure;

public class Stacks : IStacks
{
    readonly IMongoCollection<StackDeploymentForApplication> _collection;
    readonly ILogger<Stacks> _logger;

    public Stacks(IMongoCollection<StackDeploymentForApplication> collection, ILogger<Stacks> logger)
    {
        _collection = collection;
        _logger = logger;
    }

    public async Task<bool> HasFor(ApplicationId applicationId)
    {
        var count = await _collection.CountDocumentsAsync(_ => _.Id == applicationId);
        return count == 1;
    }

    public async Task<StackDeployment> GetFor(ApplicationId applicationId)
    {
        _logger.Getting(applicationId);
        var result = await _collection.FindAsync(_ => _.Id == applicationId);
        var document = result.First();
        return StackDeployment.FromJsonString(document.Deployment.ToJson());
    }

    public async Task Save(ApplicationId applicationId, StackDeployment stackDeployment)
    {
        _logger.Saving(applicationId);
        try
        {
            var stackDeploymentBson = BsonDocument.Parse(stackDeployment.Json.ToJson());
            var document = new StackDeploymentForApplication(applicationId, stackDeploymentBson);
            await _collection.ReplaceOneAsync(_ => _.Id == applicationId, document, new ReplaceOptions { IsUpsert = true });
        }
        catch (Exception ex)
        {
            _logger.ErrorSaving(applicationId, ex);
            throw;
        }
    }
}
