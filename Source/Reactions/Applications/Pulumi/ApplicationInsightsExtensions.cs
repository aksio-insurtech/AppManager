// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Pulumi.AzureNative.OperationalInsights;
using Pulumi.AzureNative.OperationalInsights.Inputs;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationInsightsExtensions
{
    public static Workspace SetupApplicationInsights(
        this Application application,
        ResourceGroup resourceGroup,
        CloudRuntimeEnvironment environment,
        Tags tags)
    {
        return new Workspace(application.Name, new()
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
    }
}
