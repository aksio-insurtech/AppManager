// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationResourceGroupPulumiExtensions
{
    public static ResourceGroup SetupResourceGroup(this Application application, ApplicationEnvironment environment)
    {
        var name = GetResourceGroupName(application, environment);
        return new ResourceGroup(name, new()
        {
            Location = application.CloudLocation.Value,
            ResourceGroupName = name,
            Tags = application.GetTags(environment)
        });
    }

    public static ResourceGroup GetResourceGroup(this Application application, ApplicationEnvironment environment)
    {
        return ResourceGroup.Get(GetResourceGroupName(application, environment), application.Resources.AzureResourceGroupId.Value);
    }

    static string GetResourceGroupName(Application application, ApplicationEnvironment environment)
    {
        var locationString = () => application.CloudLocation.Value switch
        {
            CloudLocationKey.NorwayEast => "Norway",
            CloudLocationKey.EuropeWest => "Netherlands",
            CloudLocationKey.EuropeNorth => "Ireland",
            _ => "NA"
        };

        return $"{application.Name}-{environment.ShortName}-{locationString()}-RG";
    }
}
