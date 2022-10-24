// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Pulumi.Automation;

namespace Infrastructure;

public class StacksForMicroservices : IStacksForMicroservices
{
    readonly IMongoCollection<StackDeploymentForMicroservice> _collection;
    readonly ILogger<StacksForMicroservices> _logger;

    public StacksForMicroservices(IMongoCollection<StackDeploymentForMicroservice> collection, ILogger<StacksForMicroservices> logger)
    {
        _collection = collection;
        _logger = logger;
    }

    public async Task<bool> HasFor(
        ApplicationId applicationId,
        MicroserviceId microserviceId,
        ApplicationEnvironment environment)
    {
        var count = await _collection.CountDocumentsAsync(_ => _.Id == microserviceId && _.Environment == environment.DisplayName);
        return count == 1;
    }

    public async Task<StackDeployment> GetFor(
        ApplicationId applicationId,
        MicroserviceId microserviceId,
        ApplicationEnvironment environment)
    {
        _logger.Getting(microserviceId);
        var result = await _collection.FindAsync(_ => _.Id == microserviceId && _.Environment == environment.DisplayName);
        var document = result.First();
        return StackDeployment.FromJsonString(document.Deployment.ToString());
    }

    public async Task Save(
        ApplicationId applicationId,
        MicroserviceId microserviceId,
        ApplicationEnvironment environment,
        StackDeployment stackDeployment)
    {
        _logger.Saving(microserviceId);
        try
        {
            var stackDeploymentBson = BsonDocument.Parse(stackDeployment.Json.ToString());
            var document = new StackDeploymentForMicroservice(microserviceId, environment.DisplayName, stackDeploymentBson);
            await _collection.ReplaceOneAsync(_ => _.Id == microserviceId && _.Environment == environment.DisplayName, document, new ReplaceOptions { IsUpsert = true });
        }
        catch (Exception ex)
        {
            _logger.ErrorSaving(microserviceId, ex);
            throw;
        }
    }
}
