// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Pulumi.AzureNative.ContainerRegistry;
using Pulumi.AzureNative.ContainerRegistry.Inputs;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationContainerRegistryPulumiExtensions
{
    public static async Task<ContainerRegistryResult> SetupContainerRegistry(this Application application, ResourceGroup resourceGroup, Tags tags)
    {
        var registry = new Registry(application.Name.Value.ToLowerInvariant(), new RegistryArgs
        {
            ResourceGroupName = resourceGroup.Name,

            // Todo: We force this, due to Norway not supporting the GetRegistryCredentials API for some reason.
            Location = CloudLocationKey.EuropeWest,
            Tags = tags,
            Sku = new SkuArgs
            {
                Name = SkuName.Standard
            },
            AdminUserEnabled = true,
        });

        var registryCredentials = GetRegistryCredentials.Invoke(new()
        {
            ResourceGroupName = resourceGroup.Name,
            RegistryName = registry.Name
        });

        var loginServer = await registry.LoginServer.GetValue();
        var registryCredentialsResult = await registryCredentials.GetValue();

        return new(
            registry,
            loginServer,
            registryCredentialsResult.Username ?? string.Empty,
            registryCredentialsResult.Password ?? string.Empty);
    }
}
