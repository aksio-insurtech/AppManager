// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.Network;
using Pulumi.AzureNative.Network.Inputs;
using Pulumi.AzureNative.Resources;
using NetworkSecurityGroupArgs = Pulumi.AzureNative.Network.Inputs.NetworkSecurityGroupArgs;

namespace Reactions.Applications.Pulumi;

public static class ApplicationNetworkPulumiExtensions
{
    public static NetworkResult SetupNetwork(this Application application, ResourceGroup resourceGroup, Tags tags)
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
            EnableDdosProtection = false,
            EnableVmProtection = false,
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
                        NetworkSecurityGroup = new NetworkSecurityGroupArgs
                        {
                            Id = securityGroup.Id
                        },
                        ServiceEndpoints =
                        {
                            new ServiceEndpointPropertiesFormatArgs
                            {
                                Service = "Microsoft.Storage"
                            },
                            new ServiceEndpointPropertiesFormatArgs
                            {
                                Service = "Microsoft.KeyVault"
                            }
                        },
                        AddressPrefix = "10.0.0.0/24",
                        Name = application.Name.Value,
                        PrivateEndpointNetworkPolicies = "Enabled",
                        PrivateLinkServiceNetworkPolicies = "Enabled",
                        Delegations =
                        {
                            new DelegationArgs
                            {
                                Name = "containerGroupDelegation",
                                ServiceName = "Microsoft.ContainerInstance/containerGroups"
                            }
                        }
                    }
                }
        });

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
                                Id = virtualNetwork.Subnets.Apply(_ => _[0].Id!)
                            }
                        }
                    }
                }
            },
        });

        return new(securityGroup, virtualNetwork, profile);
    }
}
