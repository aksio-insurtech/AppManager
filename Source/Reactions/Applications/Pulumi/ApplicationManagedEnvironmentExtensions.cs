// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Azure;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Pulumi.AzureNative.App;

namespace Reactions.Applications.Pulumi;

public static class ApplicationManagedEnvironmentExtensions
{
    public static async Task<ManagedEnvironment> GetManagedEnvironment(
        this Application application,
        ApplicationEnvironmentWithArtifacts environment,
        AzureServicePrincipal servicePrincipal,
        AzureSubscription subscription)
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

        var resourceGroupName = application.GetResourceGroupName(environment, environment.CloudLocation);
        var resources = await azure.GenericResources.ListByResourceGroupAsync(resourceGroupName);
        var managedEnvironment = resources.First(_ => _.ResourceType == "managedEnvironments");
        return ManagedEnvironment.Get(application.Name, managedEnvironment.Id);
    }
}
