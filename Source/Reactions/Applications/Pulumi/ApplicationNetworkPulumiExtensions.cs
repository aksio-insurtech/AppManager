// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi;
using Pulumi.AzureNative.Network;
using Pulumi.AzureNative.Network.Inputs;
using Pulumi.AzureNative.Resources;
using RouteArgs = Pulumi.AzureNative.Network.Inputs.RouteArgs;

namespace Reactions.Applications.Pulumi;

public static class ApplicationNetworkPulumiExtensions
{
    public static string GetPrivateZoneName(this Application application) => $"{application.Name}.local".ToLowerInvariant();

    public static NetworkResult SetupNetwork(this Application application, ResourceGroup resourceGroup, Tags tags)
    {
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
                        Name = "internal",
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
                        AddressPrefix = "10.0.1.0/24",
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
                    },
                    new global::Pulumi.AzureNative.Network.Inputs.SubnetArgs
                    {
                        Name = "AzureFirewallSubnet",
                        AddressPrefix = "10.0.2.0/24",
                    }
                }
        });

        var privateZoneName = application.GetPrivateZoneName();
        var privateZone = new PrivateZone("privateZone", new()
        {
            Location = "Global",
            PrivateZoneName = privateZoneName,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
        });

        var virtualNetworkLink = new VirtualNetworkLink("virtualNetworkLink", new()
        {
            Location = "Global",
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            RegistrationEnabled = true,
            PrivateZoneName = privateZone.Name,
            VirtualNetwork = new SubResourceArgs
            {
                Id = virtualNetwork.Id
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

        var publicIPAddress = new PublicIPAddress(application.Name, new()
        {
            Location = application.CloudLocation.Value,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            PublicIPAddressVersion = "IPv4",
            PublicIPAllocationMethod = "Static",
            PublicIpAddressName = "public",
            Sku = new PublicIPAddressSkuArgs
            {
                Name = PublicIPAddressSkuName.Standard,
                Tier = PublicIPAddressSkuTier.Regional
            }
        });

        var firewall = new AzureFirewall(application.Name, new()
        {
            Location = application.CloudLocation.Value,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            IpConfigurations = new AzureFirewallIPConfigurationArgs
            {
                Name = "public",
                PublicIPAddress = new SubResourceArgs
                {
                    Id = publicIPAddress.Id
                },
                Subnet = new SubResourceArgs
                {
                    Id = virtualNetwork.Subnets.Apply(_ => _[1].Id!)
                }
            },
            NatRuleCollections =
            {
                new AzureFirewallNatRuleCollectionArgs
                {
                    Action = new AzureFirewallNatRCActionArgs
                    {
                        Type = AzureFirewallNatRCActionType.Dnat
                    },
                    Name = "webnatrulecollection",
                    Priority = 112,
                    Rules =
                    {
                        new AzureFirewallNatRuleArgs
                        {
                            Name = "DNAT-HTTP-traffic-with-FQDN",
                            Description = "D-NAT all inbound web traffic",
                            DestinationAddresses =
                            {
                                publicIPAddress.IpAddress.Apply(_ => _!)
                            },
                            DestinationPorts =
                            {
                                "80"
                            },
                            Protocols =
                            {
                                AzureFirewallNetworkRuleProtocol.TCP
                            },
                            SourceAddresses =
                            {
                                "*"
                            },
                            TranslatedFqdn = $"ingress.{privateZoneName}",
                            TranslatedPort = "80"
                        }
                    }
                }
            }
        });

        var routeTable = new RouteTable(application.Name, new()
        {
            Location = application.CloudLocation.Value,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            DisableBgpRoutePropagation = true,
            RouteTableName = $"{application.Name}Routes",
            Routes =
            {
                new RouteArgs
                {
                    Name = "firewall",
                    AddressPrefix = "0.0.0.0/0",
                    NextHopType = RouteNextHopType.VirtualAppliance,
                    NextHopIpAddress = firewall.IpConfigurations.Apply(_ => _[0].PrivateIPAddress)
                }
            }
        });

        return new(virtualNetwork, profile, privateZone, publicIPAddress, firewall);
    }
}
