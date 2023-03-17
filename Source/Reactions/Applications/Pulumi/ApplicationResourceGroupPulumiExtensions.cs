// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationResourceGroupPulumiExtensions
{
    public static ResourceGroup SetupResourceGroup(
        this Application application,
        ApplicationEnvironmentWithArtifacts environment)
    {
        var name = GetResourceGroupName(application, environment, environment.CloudLocation);
        return new ResourceGroup(
            name,
            new()
            {
                Location = environment.CloudLocation.Value,
                ResourceGroupName = name,
                Tags = application.GetTags(environment)
            });
    }

    public static ResourceGroup GetResourceGroup(
        this Application application,
        ApplicationEnvironmentWithArtifacts environment)
    {
        var result = global::Pulumi.AzureNative.Resources.GetResourceGroup.Invoke(new()
        {
            ResourceGroupName = GetResourceGroupName(application, environment, environment.CloudLocation)
        });
        var resourceGroupId = result.Apply(_ => _.Id);
        return ResourceGroup.Get(
            GetResourceGroupName(application, environment, environment.CloudLocation),
            resourceGroupId);
    }

    public static string GetResourceGroupName(this Application application, ApplicationEnvironment environment, CloudLocationKey cloudLocation)
    {
        var locationString = () => cloudLocation.Value switch
        {
            CloudLocationKey.NorwayEast => "Norway",
            CloudLocationKey.EuropeWest => "Netherlands",
            CloudLocationKey.EuropeNorth => "Ireland",
            _ => "NA"
        };

        return $"{application.Name}-{environment.ShortName}-{locationString()}-RG";
    }
}
