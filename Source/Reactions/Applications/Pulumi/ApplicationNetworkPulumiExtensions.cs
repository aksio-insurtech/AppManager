// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.Network;
using Pulumi.AzureNative.Network.Inputs;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationNetworkPulumiExtensions
{
    public static async Task<NetworkResult> SetupNetwork(this Application application, ResourceGroup resourceGroup, Tags tags)
    {
        var securityGroup = new NetworkSecurityGroup(application.Name, new()
        {
            Location = application.CloudLocation.Value,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags
        });

        var virtualNetwork = new VirtualNetwork(application.Name, new()
        {
            Location = application.CloudLocation.Value,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            AddressSpace = new AddressSpaceArgs
            {
                AddressPrefixes =
                    {
                        "10.0.0.0/16"
                    }
            },
            Subnets =
                {
                    new global::Pulumi.AzureNative.Network.Inputs.SubnetArgs
                    {
                        AddressPrefix = "10.0.0.0/24",
                        Name = application.Name.Value
                    }
                }
        });

        var subnets = await virtualNetwork.Subnets.GetValue();
        var profile = new NetworkProfile(application.Name, new()
        {
            Location = application.CloudLocation.Value,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            ContainerNetworkInterfaceConfigurations =
            {
                new ContainerNetworkInterfaceConfigurationArgs
                {
                    Name = "eth1",
                    IpConfigurations =
                    {
                        new IPConfigurationProfileArgs
                        {
                            Name = "ipconfig1",
                            Subnet = new global::Pulumi.AzureNative.Network.Inputs.SubnetArgs
                            {
                                Id = subnets[0].Id ?? string.Empty
                            }
                        }
                    }
                }
            },
        });

        return new(securityGroup, virtualNetwork, profile);
    }
}
