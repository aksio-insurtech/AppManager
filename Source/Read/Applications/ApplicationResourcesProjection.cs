// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;

namespace Read.Applications;

public class ApplicationResourcesProjection : IProjectionFor<ApplicationResources>
{
    public ProjectionId Identifier => "6390f7ab-67c1-4b18-ac61-79b92c8a8352";

    public void Define(IProjectionBuilderFor<ApplicationResources> builder) => builder
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
        .From<IpAddressSetForApplication>(_ => _
            .Set(m => m.IpAddress).To(e => e.Address))
        .RemovedWith<ApplicationRemoved>();
}
