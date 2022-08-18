// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Concepts.Applications;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationResourceGroupPulumiExtensions
{
    public static ResourceGroup SetupResourceGroup(this Application application, CloudRuntimeEnvironment environment)
    {
        var locationString = () => application.CloudLocation.Value switch
        {
            CloudLocationKey.NorwayEast => "Norway",
            CloudLocationKey.EuropeWest => "Netherlands",
            CloudLocationKey.EuropeNorth => "Ireland",
            _ => "NA"
        };

        var name = $"{application.Name}-{environment.ToShortName()}-{locationString()}-RG";

        return new ResourceGroup(name, new()
        {
            Location = application.CloudLocation.Value,
            ResourceGroupName = name,
            Tags = application.GetTags(environment)
        });
    }
}
