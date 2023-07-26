// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Reactive.Subjects;
using Aksio.Execution;
using Concepts.Applications.Environments;

namespace Read.Applications.Environments;

/// <summary>
/// Represents an implementation of <see cref="IApplicationEnvironmentDeploymentLog"/>.
/// </summary>
[Singleton]
public class ApplicationEnvironmentDeploymentLog : IApplicationEnvironmentDeploymentLog
{
    readonly IMongoCollection<ApplicationEnvironmentDeployment> _collection;
    readonly ConcurrentDictionary<ApplicationEnvironmentDeploymentId, BehaviorSubject<string>> _deployments = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationEnvironmentDeploymentLog"/> class.
    /// </summary>
    /// <param name="collection"><see cref="IMongoCollection{T}"/> for <see cref="ApplicationEnvironmentDeployment"/>.</param>
    public ApplicationEnvironmentDeploymentLog(IMongoCollection<ApplicationEnvironmentDeployment> collection)
    {
        _collection = collection;
    }

    /// <inheritdoc/>
    public IObservable<string> LogFor(ApplicationId applicationId, ApplicationEnvironmentId environmentId, ApplicationEnvironmentDeploymentId deploymentId)
    {
        if (_deployments.TryGetValue(deploymentId, out var deployment))
        {
            return deployment;
        }

        var key = new ApplicationEnvironmentDeploymentKey(applicationId, environmentId, deploymentId);
        var storedDeployment = _collection.Find(_ => _.Id == key).FirstOrDefault();
        if (storedDeployment is not null)
        {
            deployment = new BehaviorSubject<string>(storedDeployment.Log);
        }
        else
        {
            deployment = new BehaviorSubject<string>(string.Empty);
        }
        _deployments[deploymentId] = deployment;
        return deployment;
    }

    /// <inheritdoc/>
    public void Append(
        ApplicationId applicationId,
        ApplicationEnvironmentId environmentId,
        ApplicationEnvironmentDeploymentId deploymentId,
        string message)
    {
        var key = new ApplicationEnvironmentDeploymentKey(applicationId, environmentId, deploymentId);
        if (_deployments.TryGetValue(deploymentId, out var deployment))
        {
            deployment.OnNext($"{deployment.Value}\n{message}");
        }
        else
        {
            var storedDeployment = _collection.Find(_ => _.Id == key).FirstOrDefault();
            if (storedDeployment is not null)
            {
                deployment = new BehaviorSubject<string>(storedDeployment.Log);
                deployment.OnNext($"{storedDeployment.Log}\n{message}");
            }
            else
            {
                deployment = new BehaviorSubject<string>(message);
            }
        }

        _ = Task.Run(async () =>
        {
            var update = Builders<ApplicationEnvironmentDeployment>.Update
                .Set(_ => _.Log, deployment.Value);

            await _collection.UpdateOneAsync(
                _ => _.Id == key,
                update,
                new UpdateOptions { IsUpsert = true });
        });

        _deployments[deploymentId] = deployment;
    }
}
