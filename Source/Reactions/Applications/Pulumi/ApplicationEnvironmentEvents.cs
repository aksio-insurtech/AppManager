// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments;

namespace Reactions.Applications.Pulumi;

#pragma warning disable RCS1175

public static class ApplicationEnvironmentEvents
{
    public static async Task<IEnumerable<object>> GetEventsToAppend(this Application application, ApplicationEnvironmentWithArtifacts environment, ApplicationEnvironmentResult applicationResult)
    {
        var events = new List<object>();

        if (environment.ApplicationResources?.MongoDB?.ConnectionString is null ||
        environment.ApplicationResources?.MongoDB?.ConnectionString.Value != applicationResult.MongoDB.ConnectionString)
        {
            events.Add(new MongoDBConnectionStringChangedForApplicationEnvironment(applicationResult.MongoDB.ConnectionString));
        }

        if (environment.ApplicationResources?.MongoDB?.Users is null ||
            !(environment.ApplicationResources?.MongoDB?.Users.Any(_ => _.UserName == "kernel") ?? false))
        {
            events.Add(new MongoDBUserChanged("kernel", applicationResult.MongoDB.Password));
        }

        var resourceGroupId = await applicationResult.ResourceGroup.Id.GetValue();
        if (environment.ApplicationResources?.AzureResourceGroupId != resourceGroupId)
        {
            events.Add(new AzureResourceGroupCreatedForApplicationEnvironment(environment.AzureSubscriptionId, resourceGroupId));
        }

        if (environment.ApplicationResources?.AzureStorageAccountName != applicationResult.Storage.AccountName)
        {
            events.Add(new AzureStorageAccountSetForApplicationEnvironment(applicationResult.Storage.AccountName));
        }

        var subnets = await applicationResult.Network.VirtualNetwork.Subnets.GetValue();
        if (environment.ApplicationResources?.AzureVirtualNetworkIdentifier is null ||
            environment.ApplicationResources?.AzureVirtualNetworkIdentifier != subnets[0].Id!)
        {
            events.Add(new AzureVirtualNetworkIdentifierSetForApplicationEnvironment(subnets[0].Id!));
        }

        if (environment.ApplicationResources?.AzureContainerRegistryLoginServer != applicationResult.ContainerRegistry.LoginServer ||
            environment.ApplicationResources?.AzureContainerRegistryUserName != applicationResult.ContainerRegistry.UserName ||
            environment.ApplicationResources?.AzureContainerRegistryPassword != applicationResult.ContainerRegistry.Password)
        {
            events.Add(new AzureContainerRegistrySetForApplicationEnvironment(
                applicationResult.ContainerRegistry.LoginServer,
                applicationResult.ContainerRegistry.UserName,
                applicationResult.ContainerRegistry.Password));
        }

        return events;
    }
}
