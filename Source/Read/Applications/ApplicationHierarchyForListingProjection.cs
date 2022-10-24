// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;
using Events.Applications.Environments;
using Events.Applications.Environments.Ingresses;
using Events.Applications.Environments.Microservices;
using Events.Applications.Environments.Microservices.Deployables;

namespace Read.Applications;

public class ApplicationHierarchyForListingProjection : IProjectionFor<ApplicationHierarchyForListing>
{
    public ProjectionId Identifier => "5a52002b-f36e-4d69-8900-cd133de4aac3";

    public void Define(IProjectionBuilderFor<ApplicationHierarchyForListing> builder) => builder
        .From<ApplicationCreated>(_ => _
            .Set(m => m.Name).To(e => e.Name))
        .Children(_ => _.Environments, _ => _
            .IdentifiedBy(m => m.EnvironmentId)
            .From<ApplicationEnvironmentCreated>(_ => _
                .UsingParentKey(e => e.ApplicationId)
                .Set(m => m.Name).To(e => e.Name))
            .Children(_ => _.Tenants, _ => _
                .IdentifiedBy(m => m.TenantId)
                .From<TenantAddedToApplicationEnvironment>(_ => _
                    .UsingParentKey(e => e.EnvironmentId)
                    .Set(m => m.Name).To(e => e.Name)))
            .Children(_ => _.Ingresses, _ => _
                .IdentifiedBy(m => m.IngressId)
                .From<IngressCreated>(_ => _
                    .UsingParentKey(e => e.EnvironmentId)
                    .Set(m => m.Name).To(e => e.Name)))
            .Children(_ => _.Microservices, _ => _
                .IdentifiedBy(m => m.MicroserviceId)
                .From<MicroserviceCreated>(_ => _
                    .UsingParentKey(e => e.EnvironmentId)
                    .Set(m => m.Name).To(e => e.Name))
                .RemovedWith<MicroserviceRemoved>()
                .Children(_ => _.Deployables, _ => _
                    .IdentifiedBy(m => m.DeployableId)
                    .From<DeployableCreated>(_ => _
                        .UsingParentKey(e => e.MicroserviceId)
                        .Set(m => m.Name).To(e => e.Name))))
        .RemovedWith<ApplicationRemoved>());
}
