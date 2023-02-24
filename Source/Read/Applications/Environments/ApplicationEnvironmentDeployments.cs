// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Read.Applications.Environments;

[Route("/api/applications/{applicationId}/environments/{environmentId}/deployments")]
public class ApplicationEnvironmentDeployments : Controller
{
    readonly IApplicationEnvironmentDeploymentLog _deploymentLog;
    readonly IMongoCollection<ApplicationEnvironmentDeployment> _deployments;

    public ApplicationEnvironmentDeployments(
        IApplicationEnvironmentDeploymentLog deploymentLog,
        IMongoCollection<ApplicationEnvironmentDeployment> deployments)
    {
        _deploymentLog = deploymentLog;
        _deployments = deployments;
    }

    [HttpGet]
    public Task<ClientObservable<IEnumerable<ApplicationEnvironmentDeployment>>> Deployments(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId)
    {
        var sort = new SortDefinitionBuilder<ApplicationEnvironmentDeployment>().Descending(_ => _.Started);
        return _deployments.Observe(
            deployment => deployment.Id.ApplicationId == applicationId && deployment.Id.EnvironmentId == environmentId,
            new FindOptions<ApplicationEnvironmentDeployment, ApplicationEnvironmentDeployment> { Sort = sort });
    }

    [HttpGet("{deploymentId}")]
    public Task<ClientObservable<LogMessage>> Deployment(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] ApplicationEnvironmentDeploymentId deploymentId)
    {
        var observable = new ClientObservable<LogMessage>();
        var log = _deploymentLog.LogFor(applicationId, environmentId, deploymentId);
        log.Subscribe(_ => observable.OnNext(new(_)));
        return Task.FromResult(observable);
    }
}
