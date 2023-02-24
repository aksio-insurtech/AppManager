// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Read.Applications.Environments;

[Route("/api/applications/{applicationId}/environments/{environmentId}/consolidations")]
public class ApplicationEnvironmentConsolidations : Controller
{
    readonly IApplicationEnvironmentConsolidationLog _consolidationLog;
    readonly IMongoCollection<ApplicationEnvironmentConsolidation> _consolidations;

    public ApplicationEnvironmentConsolidations(
        IApplicationEnvironmentConsolidationLog consolidationLog,
        IMongoCollection<ApplicationEnvironmentConsolidation> consolidations)
    {
        _consolidationLog = consolidationLog;
        _consolidations = consolidations;
    }

    [HttpGet]
    public Task<ClientObservable<IEnumerable<ApplicationEnvironmentConsolidation>>> Consolidations(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId)
    {
        var sort = new SortDefinitionBuilder<ApplicationEnvironmentConsolidation>().Descending(_ => _.Started);
        return _consolidations.Observe(
            consolidation => consolidation.Id.ApplicationId == applicationId && consolidation.Id.EnvironmentId == environmentId,
            new FindOptions<ApplicationEnvironmentConsolidation, ApplicationEnvironmentConsolidation> { Sort = sort });
    }

    [HttpGet("{consolidationId}")]
    public Task<ClientObservable<LogMessage>> Consolidation(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] ApplicationEnvironmentConsolidationId consolidationId)
    {
        var observable = new ClientObservable<LogMessage>();
        var log = _consolidationLog.LogFor(applicationId, environmentId, consolidationId);
        log.Subscribe(_ => observable.OnNext(new(_)));
        return Task.FromResult(observable);
    }
}
