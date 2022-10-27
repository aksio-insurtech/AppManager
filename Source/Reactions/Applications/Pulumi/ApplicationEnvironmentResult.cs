// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.App;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public record ApplicationEnvironmentResult(
    ApplicationEnvironmentWithArtifacts Environment,
    ResourceGroup ResourceGroup,
    NetworkResult Network,
    StorageResult Storage,
    ContainerRegistryResult ContainerRegistry,
    MongoDBResult MongoDB,
    ManagedEnvironment ManagedEnvironment,
    ContainerAppResult Kernel)
{
    public async Task<ApplicationEnvironmentWithArtifacts> MergeWithApplicationEnvironment(ApplicationEnvironmentWithArtifacts environment)
    {
        var subnets = await Network.VirtualNetwork.Subnets.GetValue();
        return new(
            environment.Id,
            environment.Name,
            environment.DisplayName,
            environment.ShortName,
            environment.CratisVersion,
            environment.AzureSubscriptionId,
            environment.CloudLocation,
            new(
                subnets[0].Id!,
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
            environment.Tenants,
            environment.Ingresses,
            environment.Microservices);
    }
}
