// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Azure;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationResourceGroupPulumiExtensions
{
    public static async Task<ResourceGroup> SetupResourceGroup(this Application application, ApplicationEnvironmentWithArtifacts environment, AzureServicePrincipal servicePrincipal, AzureSubscription subscription)
    {
        var name = GetResourceGroupName(application, environment);
        if (!PulumiOperations.CurrentStack.HasResourceGroup())
        {
            var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(
                clientId: servicePrincipal.ClientId,
                clientSecret: servicePrincipal.ClientSecret,
                tenantId: subscription.TenantId,
                environment: AzureEnvironment.AzureGlobalCloud);

            var azure = Microsoft.Azure.Management.Fluent.Azure
                .Configure()
                .Authenticate(credentials)
                .WithSubscription(subscription.SubscriptionId);

            var resourceGroups = await azure.ResourceGroups.ListAsync();
            var resourceGroup = resourceGroups.FirstOrDefault(_ => _.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (resourceGroup is not null)
            {
                return ResourceGroup.Get(name, resourceGroup.Id);
            }
        }

        return new ResourceGroup(name, new()
        {
            Location = environment.CloudLocation.Value,
            ResourceGroupName = name,
            Tags = application.GetTags(environment)
        });
    }

    public static ResourceGroup GetResourceGroup(this Application application, ApplicationEnvironmentWithArtifacts environment)
    {
        return ResourceGroup.Get(GetResourceGroupName(application, environment), environment.ApplicationResources.AzureResourceGroupId.Value);
    }

    static string GetResourceGroupName(Application application, ApplicationEnvironmentWithArtifacts environment)
    {
        var locationString = () => environment.CloudLocation.Value switch
        {
            CloudLocationKey.NorwayEast => "Norway",
            CloudLocationKey.EuropeWest => "Netherlands",
            CloudLocationKey.EuropeNorth => "Ireland",
            _ => "NA"
        };

        return $"{application.Name}-{environment.ShortName}-{locationString()}-RG";
    }
}
