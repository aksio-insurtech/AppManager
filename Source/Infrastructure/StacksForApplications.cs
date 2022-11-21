// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Pulumi.Automation;

namespace Infrastructure;

public class StacksForApplications : IStacksForApplications
{
    readonly IMongoCollection<StackDeploymentForApplication> _collection;
    readonly ILogger<StacksForApplications> _logger;

    public StacksForApplications(IMongoCollection<StackDeploymentForApplication> collection, ILogger<StacksForApplications> logger)
    {
        _collection = collection;
        _logger = logger;
    }

    public async Task<bool> HasFor(ApplicationId applicationId, ApplicationEnvironment environment)
    {
        var count = await _collection.CountDocumentsAsync(_ => _.Id == applicationId && _.Environment == environment.DisplayName);
        return count == 1;
    }

    public async Task<StackDeployment> GetFor(ApplicationId applicationId, ApplicationEnvironment environment)
    {
        _logger.Getting(applicationId);
        var result = await _collection.FindAsync(_ => _.Id == applicationId && _.Environment == environment.DisplayName);
        var document = result.First();
        return StackDeployment.FromJsonString(document.Deployment.ToString());
    }

    public async Task Save(ApplicationId applicationId, ApplicationEnvironment environment, StackDeployment stackDeployment)
    {
        _logger.Saving(applicationId);
        try
        {
            var stackDeploymentBson = BsonDocument.Parse(stackDeployment.Json.ToString());
            var document = new StackDeploymentForApplication(applicationId, environment.DisplayName, stackDeploymentBson);
            await _collection.ReplaceOneAsync(_ => _.Id == applicationId && _.Environment == environment.DisplayName, document, new ReplaceOptions { IsUpsert = true });
        }
        catch (Exception ex)
        {
            _logger.ErrorSaving(applicationId, ex);
            throw;
        }
    }
}
