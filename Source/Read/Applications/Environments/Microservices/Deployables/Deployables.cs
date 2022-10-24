// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Read.Applications;

[Route("/api/applications/{applicationId}/environments/{environmentId}/microservices/{microserviceId}/deployables")]
public class Deployables : Controller
{
    readonly IMongoCollection<Deployable> _deployableCollection;

    public Deployables(IMongoCollection<Deployable> deployableCollection) =>
        _deployableCollection = deployableCollection;

    [HttpGet]
    public Task<ClientObservable<IEnumerable<Deployable>>> DeployablesForMicroservice(
        [FromRoute] ApplicationId applicationId,
        [FromRoute] ApplicationEnvironmentId environmentId,
        [FromRoute] MicroserviceId microserviceId) =>
            _deployableCollection.Observe(Builders<Deployable>.Filter.And(
                Filters.StringFilterFor<Deployable>(_ => _.Id.ApplicationId, applicationId),
                Filters.StringFilterFor<Deployable>(_ => _.Id.EnvironmentId, environmentId),
                Filters.StringFilterFor<Deployable>(_ => _.Id.MicroserviceId, microserviceId)));
}
