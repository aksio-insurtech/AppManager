// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
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
        NetworkResult network,
        Tags tags)
    {
        var sharedKeysResult = GetSharedKeys.Invoke(new()
        {
            ResourceGroupName = resourceGroup.Name,
            WorkspaceName = applicationInsights.Name
        });

        return new ManagedEnvironment(application.Name, new()
        {
            Location = resourceGroup.Location,
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

            // VnetConfiguration = new VnetConfigurationArgs()
            // {
            //     DockerBridgeCidr = "10.2.0.1/16",
            //     PlatformReservedCidr = "10.0.0.0/16",
            //     PlatformReservedDnsIP = "10.0.0.2",
            //     RuntimeSubnetId = network.VirtualNetwork.Subnets.Apply(_ => _[0].Id!),
            //     InfrastructureSubnetId = network.VirtualNetwork.Subnets.Apply(_ => _[1].Id!),
            //     Internal = true
            // },
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
