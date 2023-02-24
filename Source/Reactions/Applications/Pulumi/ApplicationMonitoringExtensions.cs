// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Pulumi.AzureNative.Insights.V20200202;
using Pulumi.AzureNative.OperationalInsights;
using Pulumi.AzureNative.OperationalInsights.Inputs;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationMonitoringExtensions
{
    public static ApplicationMonitoringResult SetupApplicationMonitoring(
        this Application application,
        ResourceGroup resourceGroup,
        ApplicationEnvironment environment,
        Tags tags)
    {
        var workspace = new Workspace(application.Name, new()
        {
            Location = resourceGroup.Location,
            ResourceGroupName = resourceGroup.Name,
            RetentionInDays = 30,
            Tags = tags,
            Sku = new WorkspaceSkuArgs
            {
                Name = WorkspaceSkuNameEnum.PerGB2018
            }
        });

        var component = new Component(application.Name, new()
        {
            ApplicationType = "web",
            FlowType = "Bluefield",
            Kind = "web",
            Location = resourceGroup.Location,
            RequestSource = "rest",
            ResourceGroupName = resourceGroup.Name,
            RetentionInDays = 30,
            IngestionMode = IngestionMode.LogAnalytics,
            WorkspaceResourceId = workspace.Id,
            Tags = tags
        });

        return new(workspace, component);
    }
}
