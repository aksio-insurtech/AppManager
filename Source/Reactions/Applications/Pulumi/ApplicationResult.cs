// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public record ApplicationResult(
    CloudRuntimeEnvironment Environment,
    ResourceGroup ResourceGroup,
    NetworkResult Network,
    StorageResult Storage,
    ContainerRegistryResult ContainerRegistry,
    MongoDBResult MongoDB,
    ContainerAppResult Kernel)
{
    public async Task<Application> MergeWithApplication(Application application)
    {
        var subnets = await Network.VirtualNetwork.Subnets.GetValue();

        return new(
            application.Id,
            application.Name,
            application.AzureSubscriptionId,
            application.CloudLocation,
            new(
                subnets[0].Id!,
                await Network.Profile.Id.GetValue(),
                Storage.AccountName,
                ContainerRegistry.LoginServer,
                ContainerRegistry.UserName,
                ContainerRegistry.Password,
                await ResourceGroup.Id.GetValue(),
                new(
                    MongoDB.ConnectionString,
                    new MongoDBUser[]
                    {
                        new("kernel", MongoDB.Password)
                    }
                )
            ),
            application.Authentication
        );
    }
}
