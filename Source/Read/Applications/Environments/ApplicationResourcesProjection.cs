// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;
using Events.Applications.Environments;

namespace Read.Applications.Environments;

public class ApplicationResourcesProjection : IProjectionFor<ApplicationEnvironmentResources>
{
    public ProjectionId Identifier => "6390f7ab-67c1-4b18-ac61-79b92c8a8352";

    public void Define(IProjectionBuilderFor<ApplicationEnvironmentResources> builder) => builder
        .From<MongoDBConnectionStringChangedForApplication>(_ => _
            .Set(m => m.MongoDB.ConnectionString).To(e => e.ConnectionString))
        .Children(m => m.MongoDB.Users, cb => cb
            .IdentifiedBy(m => m.UserName)
            .From<MongoDBUserChanged>(e => e
                .UsingKey(e => e.UserName)
                .Set(m => m.UserName).To(e => e.UserName)
                .Set(m => m.Password).To(e => e.Password)))
        .From<AzureResourceGroupCreatedForApplication>(_ => _
            .Set(m => m.Azure.SubscriptionId).To(e => e.SubscriptionId)
            .Set(m => m.Azure.ResourceGroupId).To(e => e.ResourceGroupId))
        .From<AzureStorageAccountSetForApplication>(_ => _
            .Set(m => m.Azure.StorageAccountName).To(e => e.AccountName))
        .From<AzureContainerRegistrySetForApplication>(_ => _
            .Set(m => m.Azure.ContainerRegistryLoginServer).To(e => e.LoginServer)
            .Set(m => m.Azure.ContainerRegistryUserName).To(e => e.UserName)
            .Set(m => m.Azure.ContainerRegistryPassword).To(e => e.Password))
        .RemovedWith<ApplicationRemoved>();
}
