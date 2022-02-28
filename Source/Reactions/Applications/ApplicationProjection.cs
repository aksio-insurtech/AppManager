// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;

namespace Reactions.Applications;

public class ApplicationProjection : IPassiveProjectionFor<Application>
{
    public ProjectionId Identifier => "c2f0e081-6a1a-46e0-bc8c-8a08e0c4dff5";

    public void Define(IProjectionBuilderFor<Application> builder) => builder
        .From<ApplicationCreated>(_ => _
            .Set(m => m.Name).To(e => e.Name)
            .Set(m => m.AzureSubscriptionId).To(e => e.AzureSubscriptionId)
            .Set(m => m.CloudLocation).To(e => e.CloudLocation))
        .From<AzureResourceGroupCreatedForApplication>(_ => _
            .Set(m => m.Resources.AzureResourceGroupId).To(e => e.ResourceGroupId))
        .From<AzureStorageAccountSetForApplication>(_ => _
            .Set(m => m.Resources.AzureStorageAccountName).To(e => e.AccountName))
        .From<AzureContainerRegistrySetForApplication>(_ => _
            .Set(m => m.Resources.AzureContainerRegistryLoginServer).To(e => e.LoginServer)
            .Set(m => m.Resources.AzureContainerRegistryUserName).To(e => e.UserName)
            .Set(m => m.Resources.AzureContainerRegistryPassword).To(e => e.Password))
        .From<MongoDBConnectionStringChangedForApplication>(_ => _
            .Set(m => m.Resources.MongoDB.ConnectionString).To(e => e.ConnectionString))
        .From<IpAddressSetForApplication>(_ => _
            .Set(m => m.Resources.IpAddress).To(e => e.Address));
}
