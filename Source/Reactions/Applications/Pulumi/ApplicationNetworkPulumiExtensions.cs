// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.ManagedIdentity;
using Pulumi.AzureNative.Network;
using Pulumi.AzureNative.Network.Inputs;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationNetworkPulumiExtensions
{
    public static string GetPrivateZoneName(this Application application) => $"{application.Name}.local".ToLowerInvariant();

    public static NetworkResult SetupNetwork(this Application application, UserAssignedIdentity identity, Vault vault, ResourceGroup resourceGroup, Tags tags)
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
                            new ServiceEndpointPropertiesFormatArgs { Service = "Microsoft.Storage" },
                            new ServiceEndpointPropertiesFormatArgs { Service = "Microsoft.KeyVault" }
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
