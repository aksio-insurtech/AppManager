// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Reactive.Subjects;
using Aksio.Cratis.Execution;
using Concepts.Applications.Environments;

namespace Read.Applications.Environments;

/// <summary>
/// Represents an implementation of <see cref="IApplicationEnvironmentConsolidationLog"/>.
/// </summary>
[Singleton]
public class ApplicationEnvironmentConsolidationLog : IApplicationEnvironmentConsolidationLog
{
    readonly IMongoCollection<ApplicationEnvironmentConsolidation> _collection;
    readonly ConcurrentDictionary<ApplicationEnvironmentConsolidationId, BehaviorSubject<string>> _consolidations = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationEnvironmentConsolidationLog"/> class.
    /// </summary>
    /// <param name="collection"><see cref="IMongoCollection{T}"/> for <see cref="ApplicationEnvironmentConsolidation"/>.</param>
    public ApplicationEnvironmentConsolidationLog(IMongoCollection<ApplicationEnvironmentConsolidation> collection)
    {
        _collection = collection;
    }

    /// <inheritdoc/>
    public IObservable<string> LogFor(ApplicationId applicationId, ApplicationEnvironmentId environmentId, ApplicationEnvironmentConsolidationId consolidationId)
    {
        if (_consolidations.TryGetValue(consolidationId, out var consolidation))
        {
            return consolidation;
        }

        var key = new ApplicationEnvironmentConsolidationKey(applicationId, environmentId, consolidationId);
        var storedConsolidation = _collection.Find(_ => _.Id == key).FirstOrDefault();
        if (storedConsolidation is not null)
        {
            consolidation = new BehaviorSubject<string>(storedConsolidation.Log);
        }
        else
        {
            consolidation = new BehaviorSubject<string>(string.Empty);
        }
        _consolidations[consolidationId] = consolidation;
        return consolidation;
    }

    /// <inheritdoc/>
    public void Append(
        ApplicationId applicationId,
        ApplicationEnvironmentId environmentId,
        ApplicationEnvironmentConsolidationId consolidationId,
        string message)
    {
        var key = new ApplicationEnvironmentConsolidationKey(applicationId, environmentId, consolidationId);
        if (_consolidations.TryGetValue(consolidationId, out var consolidation))
        {
            consolidation.OnNext($"{consolidation.Value}\n{message}");
        }
        else
        {
            var storedConsolidation = _collection.Find(_ => _.Id == key).FirstOrDefault();
            if (storedConsolidation is not null)
            {
                consolidation = new BehaviorSubject<string>(storedConsolidation.Log);
                consolidation.OnNext($"{storedConsolidation.Log}\n{message}");
            }
            else
            {
                consolidation = new BehaviorSubject<string>(message);
            }
        }

        _ = Task.Run(async () =>
        {
            var update = Builders<ApplicationEnvironmentConsolidation>.Update
                .Set(_ => _.Log, consolidation.Value);

            await _collection.UpdateOneAsync(
                _ => _.Id == key,
                update,
                new UpdateOptions { IsUpsert = true });
        });

        _consolidations[consolidationId] = consolidation;
    }
}
