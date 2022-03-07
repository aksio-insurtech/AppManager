// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationResourceGroupPulumiExtensions
{
    public static ResourceGroup SetupResourceGroup(this Application application)
    {
        var resourceGroupId = $"/subscriptions/{application.AzureSubscriptionId}/resourceGroups/Einar-D-Norway-RG";
        return ResourceGroup.Get("Einar-D-Norway-RG", resourceGroupId);
    }
}
