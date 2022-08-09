// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationResourceGroupPulumiExtensions
{
    public static ResourceGroup SetupResourceGroup(this Application application, CloudRuntimeEnvironment environment)
    {
        var resourceGroupId = $"/subscriptions/{application.AzureSubscriptionId}/resourceGroups/Einar-D-Norway-RG-2";

        var environmentString = () => environment switch
        {
            CloudRuntimeEnvironment.Development => "D",
            CloudRuntimeEnvironment.Production => "D",
            _ => "U"
        };

        var locationString = () => application.CloudLocation.Value switch
        {
            "norwayeast" => "Norway",
            "westeurope" => "Netherlands",
            "northeurope" => "Ireland",
            _ => "NA"
        };

        return ResourceGroup.Get($"{application.Name}-{environmentString}-{locationString}-RG", resourceGroupId);
    }
}
