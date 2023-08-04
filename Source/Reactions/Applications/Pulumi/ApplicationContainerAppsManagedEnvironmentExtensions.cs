// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Pulumi.AzureNative.App.V20221001;
using Pulumi.AzureNative.App.V20221001.Inputs;
using Pulumi.AzureNative.OperationalInsights;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationContainerAppsManagedEnvironmentExtensions
{
    public static ManagedEnvironment SetupContainerAppManagedEnvironment(
        this Application application,
        ResourceGroup resourceGroup,
        ApplicationEnvironment environment,
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
            AppLogsConfiguration = new AppLogsConfigurationArgs
            {
                Destination = "log-analytics",
                LogAnalyticsConfiguration = new LogAnalyticsConfigurationArgs
                {
                    CustomerId = applicationInsights.CustomerId,
                    SharedKey = sharedKeysResult.Apply(_ => _.PrimarySharedKey!),
                }
            },

            VnetConfiguration = new VnetConfigurationArgs()
            {
                InfrastructureSubnetId = network.VirtualNetwork.Subnets.Apply(_ => _.First(s => s.Name == "infrastructure").Id!),
                Internal = false
            },

            // DaprAIConnectionString = applicationInsights.,
            ZoneRedundant = false
        });
    }
}
