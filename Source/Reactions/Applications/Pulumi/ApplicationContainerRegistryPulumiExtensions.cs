// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Azure;
using Microsoft.Azure.Management.ContainerRegistry.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Pulumi;
using Pulumi.AzureNative;
using Pulumi.AzureNative.ContainerRegistry;
using Pulumi.AzureNative.ContainerRegistry.Inputs;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationContainerRegistryPulumiExtensions
{
    public static void SetupContainerRegistry(
        this Application application,
        ResourceGroup resourceGroup,
        Tags tags)
    {
        _ = new Registry(application.Name.Value.ToLowerInvariant(), new RegistryArgs
        {
            ResourceGroupName = resourceGroup.Name,

            // Todo: We force this, due to Norway not supporting the GetRegistryCredentials API for some reason.
            Location = CloudLocationKey.EuropeWest.Value,
            Tags = tags,
            Sku = new SkuArgs
            {
                Name = SkuName.Basic
            },
            AdminUserEnabled = true,
        });
    }

    public static async Task<ContainerRegistryResult?> GetContainerRegistry(
        this Application application,
        ApplicationEnvironmentWithArtifacts environment,
        AzureServicePrincipal servicePrincipal,
        AzureSubscription subscription)
    {
        var resourceGroupName = application.GetResourceGroupName(environment, environment.CloudLocation);

        var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(
            clientId: servicePrincipal.ClientId,
            clientSecret: servicePrincipal.ClientSecret,
            tenantId: subscription.TenantId,
            environment: AzureEnvironment.AzureGlobalCloud);

        var azure = Microsoft.Azure.Management.Fluent.Azure
            .Configure()
            .Authenticate(credentials)
            .WithSubscription(subscription.SubscriptionId);
        var registryName = application.Name.Value.ToLowerInvariant();
        var containerRegistries = await azure.ContainerRegistries.ListByResourceGroupAsync(resourceGroupName);

        if (!containerRegistries.Any())
        {
            return null;
        }

        var containerRegistry = containerRegistries.First(_ => _.Name.StartsWith(registryName));
        var registryCredentials = await containerRegistry.GetCredentialsAsync();

        var registry = Registry.Get(
            registryName,
            containerRegistry.Id,
            new CustomResourceOptions
            {
                Provider = new Provider("subscription", new ProviderArgs
                {
                    SubscriptionId = subscription.SubscriptionId.ToString(),
                    ClientId = servicePrincipal.ClientId.Value,
                    ClientSecret = servicePrincipal.ClientSecret.Value,
                    TenantId = subscription.TenantId.Value
                })
            });

        return new(
            registry,
            containerRegistry.LoginServerUrl,
            registryCredentials.Username ?? string.Empty,
            registryCredentials.AccessKeys[AccessKeyType.Primary] ?? string.Empty);
    }
}
