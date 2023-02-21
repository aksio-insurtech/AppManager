// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Read.Applications.Environments;

[Route("/api/applications/{applicationId}/environments/{environmentId}/consolidations")]
public class ApplicationEnvironmentConsolidations : Controller
{
    readonly IMongoCollection<ApplicationEnvironmentConsolidation> _consolidations;

    public ApplicationEnvironmentConsolidations(IMongoCollection<ApplicationEnvironmentConsolidation> consolidations)
    {
        _consolidations = consolidations;
    }

    [HttpGet]
    public Task<ClientObservable<IEnumerable<ApplicationEnvironmentConsolidation>>> Consolidations(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId)
        => _consolidations.Observe(consolidation => consolidation.Id.ApplicationId == applicationId && consolidation.Id.EnvironmentId == environmentId);

    [HttpGet("{consolidationId}")]
    public Task<ClientObservable<ApplicationEnvironmentConsolidation>> Consolidation(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] ApplicationEnvironmentConsolidationId consolidationId)
        => _consolidations.ObserveById(new ApplicationEnvironmentConsolidationKey(applicationId, environmentId, consolidationId));
}
