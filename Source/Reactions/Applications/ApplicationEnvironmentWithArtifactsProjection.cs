// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments;
using Events.Applications.Environments.Ingresses;
using Events.Applications.Environments.Microservices;
using Events.Applications.Environments.Microservices.Deployables;

namespace Reactions.Applications;

public class ApplicationEnvironmentWithArtifactsProjection : IImmediateProjectionFor<ApplicationEnvironmentWithArtifacts>
{
    public ProjectionId Identifier => "c2f0e081-6a1a-46e0-bc8c-8a08e0c4dff5";

    public void Define(IProjectionBuilderFor<ApplicationEnvironmentWithArtifacts> builder) => builder
        .From<ApplicationEnvironmentCreated>(_ => _
            .Set(m => m.Id).ToEventSourceId()
            .Set(m => m.Name).To(e => e.Name)
            .Set(m => m.DisplayName).To(e => e.DisplayName)
            .Set(m => m.CratisVersion).To(e => e.CratisVersion)
            .Set(m => m.AzureSubscriptionId).To(e => e.AzureSubscriptionId)
            .Set(m => m.CloudLocation).To(e => e.CloudLocation))

        .From<AzureResourceGroupCreatedForApplicationEnvironment>(_ => _
            .Set(m => m.ApplicationResources.AzureResourceGroupId).To(e => e.ResourceGroupId))
        .From<AzureStorageAccountSetForApplicationEnvironment>(_ => _
            .Set(m => m.ApplicationResources.AzureStorageAccountName).To(e => e.AccountName))
        .From<AzureContainerRegistrySetForApplicationEnvironment>(_ => _
            .Set(m => m.ApplicationResources.AzureContainerRegistryLoginServer).To(e => e.LoginServer)
            .Set(m => m.ApplicationResources.AzureContainerRegistryUserName).To(e => e.UserName)
            .Set(m => m.ApplicationResources.AzureContainerRegistryPassword).To(e => e.Password))
        .From<MongoDBConnectionStringChangedForApplicationEnvironment>(_ => _
            .Set(m => m.ApplicationResources.MongoDB.ConnectionString).To(e => e.ConnectionString))
        .From<AzureVirtualNetworkIdentifierSetForApplicationEnvironment>(_ => _
            .Set(m => m.ApplicationResources.AzureVirtualNetworkIdentifier).To(e => e.Identifier))

        // MongoDB Users
        .Children(m => m.ApplicationResources.MongoDB.Users, cb => cb
            .IdentifiedBy(m => m.UserName)
            .From<MongoDBUserChanged>(e => e
                .UsingKey(e => e.UserName)
                .Set(m => m.UserName).To(e => e.UserName)
                .Set(m => m.Password).To(e => e.Password)))

        // Certificates
        .Children(m => m.Certificates, _ => _
            .IdentifiedBy(m => m.Id)
            .From<CertificateAddedToApplicationEnvironment>(_ => _
                .UsingKey(e => e.CertificateId)
                .Set(m => m.Name).To(e => e.Name)
                .Set(m => m.Value).To(e => e.Value)))

        // Tenants
        .Children(m => m.Tenants, _ => _
            .IdentifiedBy(m => m.Id)
            .From<TenantAddedToApplicationEnvironment>(_ => _
                .UsingKey(e => e.TenantId)
                .Set(m => m.Name).To(e => e.Name)))

        // .From<DomainAssociatedWithTenant>(_ => _
        //     .UsingKey(e => e.TenantId)
        //     .Set(m => m.Domain!.Name).To(e => e.Domain)
        //     .Set(m => m.Domain!.CertificateId).To(e => e.CertificateId))
        // .From<OnBehalfOfSetForTenant>(_ => _
        //     .UsingKey(e => e.TenantId)
        //     .Set(m => m.OnBehalfOf).To(e => e.OnBehalfOf)))

        // Ingresses
        .Children(m => m.Ingresses, _ => _
            .IdentifiedBy(m => m.Id)
            .From<IngressCreated>(_ => _
                .UsingKey(e => e.IngressId)
                .Set(m => m.Name).To(e => e.Name))

            // Routes
            .Children(m => m.Routes, _ => _
                .IdentifiedBy(m => m.Path)
                .From<RouteDefinedOnIngress>(_ => _
                    .UsingKey(e => e.Path)
                    .Set(m => m.Path).To(ev => ev.Path)
                    .Set(m => m.TargetMicroservice).To(ev => ev.TargetMicroservice)
                    .Set(m => m.TargetPath).To(ev => ev.TargetPath))))

        // Microservices
        .Children(m => m.Microservices, _ => _
            .IdentifiedBy(m => m.Id)
            .From<MicroserviceCreated>(_ => _
                .UsingKey(e => e.MicroserviceId)
                .Set(m => m.Name).To(e => e.Name))

            // Deployables
            .Children(m => m.Deployables, _ => _
                .IdentifiedBy(m => m.Id)
                .From<DeployableCreated>(_ => _
                    .UsingKey(e => e.DeployableId)
                    .Set(m => m.MicroserviceId).To(e => e.MicroserviceId)
                    .Set(m => m.Name).To(e => e.Name))
                .From<DeployableImageChanged>(_ => _
                    .UsingKey(e => e.DeployableId)
                    .Set(m => m.Image).To(e => e.Image))));
}
