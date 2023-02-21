// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Concepts.Azure;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationResourceGroupPulumiExtensions
{
    public static async Task<ResourceGroup> SetupResourceGroup(this Application application, ApplicationEnvironment environment, CloudLocationKey cloudLocation, AzureServicePrincipal servicePrincipal, AzureSubscription subscription)
    {
        var name = GetResourceGroupName(application, environment, cloudLocation);
        if (!PulumiOperations.CurrentStack.HasResourceGroup(name))
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
            Location = cloudLocation.Value,
            ResourceGroupName = name,
            Tags = application.GetTags(environment)
        });
    }

    public static ResourceGroup GetResourceGroup(this Application application, ApplicationEnvironmentWithArtifacts environment)
    {
        var result = global::Pulumi.AzureNative.Resources.GetResourceGroup.Invoke(new()
        {
            ResourceGroupName = GetResourceGroupName(application, environment, environment.CloudLocation)
        });

        var resourceGroupId = result.Apply(_ => _.Id);

        return ResourceGroup.Get(GetResourceGroupName(application, environment, environment.CloudLocation), resourceGroupId);
    }

    static string GetResourceGroupName(Application application, ApplicationEnvironment environment, CloudLocationKey cloudLocation)
    {
        var locationString = () => cloudLocation.Value switch
        {
            CloudLocationKey.NorwayEast => "Norway",
            CloudLocationKey.EuropeWest => "Netherlands",
            CloudLocationKey.EuropeNorth => "Ireland",
            _ => "NA"
        };

        return $"{application.Name}-{environment.ShortName}-{locationString()}-RG";
    }
}
