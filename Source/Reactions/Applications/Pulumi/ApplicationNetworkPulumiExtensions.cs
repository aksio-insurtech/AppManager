// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.ManagedIdentity;
using Pulumi.AzureNative.Network;
using Pulumi.AzureNative.Network.Inputs;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationNetworkPulumiExtensions
{
    public static string GetPrivateZoneName(this Application application) => $"{application.Name}.local".ToLowerInvariant();

    public static NetworkResult SetupNetwork(this Application application, ApplicationEnvironmentWithArtifacts environment, UserAssignedIdentity identity, ResourceGroup resourceGroup, Tags tags)
    {
        var virtualNetwork = new VirtualNetwork(application.Name, new()
        {
            Location = environment.CloudLocation.Value,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            EnableDdosProtection = false,
            EnableVmProtection = false,
            AddressSpace = new AddressSpaceArgs
            {
                AddressPrefixes =
                    {
                        environment.Vnet?.AddressSpace ?? "10.100.0.0/16"
                    }
            },
            Subnets =
                {
                    new global::Pulumi.AzureNative.Network.Inputs.SubnetArgs
                    {
                        Name = "infrastructure",
                        ServiceEndpoints =
                        {
                            new ServiceEndpointPropertiesFormatArgs { Service = "Microsoft.Storage" },
                            new ServiceEndpointPropertiesFormatArgs { Service = "Microsoft.ContainerRegistry" },
                            new ServiceEndpointPropertiesFormatArgs { Service = "Microsoft.KeyVault" }
                        },
                        AddressPrefix = environment.Vnet?.InfraSubnet ?? "10.100.0.0/23",
                        PrivateEndpointNetworkPolicies = VirtualNetworkPrivateEndpointNetworkPolicies.Disabled,
                        PrivateLinkServiceNetworkPolicies = VirtualNetworkPrivateLinkServiceNetworkPolicies.Disabled
                    },
                    new global::Pulumi.AzureNative.Network.Inputs.SubnetArgs
                    {
                        Name = "mongodb",
                        AddressPrefix = environment.Vnet?.MongoDbSubnet ?? "10.100.2.0/24",
                        PrivateEndpointNetworkPolicies = VirtualNetworkPrivateEndpointNetworkPolicies.Disabled,
                        PrivateLinkServiceNetworkPolicies = VirtualNetworkPrivateLinkServiceNetworkPolicies.Disabled
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

        return new(virtualNetwork, privateZone);
    }
}
