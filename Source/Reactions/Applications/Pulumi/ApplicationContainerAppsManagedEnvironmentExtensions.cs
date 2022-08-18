// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Concepts.Applications;
using Pulumi.AzureNative.App;
using Pulumi.AzureNative.App.Inputs;
using Pulumi.AzureNative.OperationalInsights;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationContainerAppsManagedEnvironmentExtensions
{
    public static ManagedEnvironment SetupContainerAppManagedEnvironment(
        this Application application,
        ResourceGroup resourceGroup,
        CloudRuntimeEnvironment environment,
        Workspace applicationInsights,
        Tags tags)
    {
        var sharedKeysResult = GetSharedKeys.Invoke(new()
        {
            ResourceGroupName = resourceGroup.Name,
            WorkspaceName = applicationInsights.Name
        });

        return new ManagedEnvironment(application.Name, new()
        {
            // Todo: We force this, due to Norway not supporting Container Apps until Q3 2022.
            Location = CloudLocationKey.EuropeWest,
            Tags = tags,
            ResourceGroupName = resourceGroup.Name,
            EnvironmentName = GetName(application, environment),
            AppLogsConfiguration = new AppLogsConfigurationArgs
            {
                Destination = "log-analytics",
                LogAnalyticsConfiguration = new LogAnalyticsConfigurationArgs
                {
                    CustomerId = applicationInsights.CustomerId,
                    SharedKey = sharedKeysResult.Apply(_ => _.PrimarySharedKey!),
                }
            },

            // DaprAIConnectionString = applicationInsights.,
            ZoneRedundant = false
        });
    }

    public static async Task<(string Id, string Name)> GetContainerAppManagedEnvironment(
        this Application application,
        ResourceGroup resourceGroup,
        CloudRuntimeEnvironment environment)
    {
        var result = GetManagedEnvironment.Invoke(new()
        {
            ResourceGroupName = resourceGroup.Name,
            EnvironmentName = GetName(application, environment)
        });

        var getManagedEnvironmentResult = await result.GetValue();
        return (getManagedEnvironmentResult.Id, getManagedEnvironmentResult.Name);
    }

    static string GetName(Application application, CloudRuntimeEnvironment environment) => $"{application.Name}-{environment.ToDisplayName()}";
}
