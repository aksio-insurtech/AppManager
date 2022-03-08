// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Azure;
using Pulumi.AzureNative.ContainerInstance;
using Pulumi.AzureNative.ContainerInstance.Inputs;
using Pulumi.AzureNative.Network;
using Pulumi.AzureNative.Network.Inputs;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class MicroserviceContainerGroupPulumiExtensions
{
    public static ContainerGroup SetupContainerGroup(
        this Microservice microservice,
        Application application,
        ResourceGroup resourceGroup,
        AzureNetworkProfileIdentifier networkProfile,
        MicroserviceStorage storage,
        IEnumerable<Deployable> deployables,
        Tags tags)
    {
        var microserviceTags = tags.Clone();
        microserviceTags["microservice"] = microservice.Id.ToString();
        microserviceTags["microserviceName"] = microservice.Name.Value;

        var containerGroup = new ContainerGroup(microservice.Name, new()
        {
            Tags = tags,
            ResourceGroupName = resourceGroup.Name,
            Volumes = new VolumeArgs[]
            {
                            new()
                            {
                                Name = "storage-config",
                                AzureFile = new AzureFileVolumeArgs()
                                {
                                    ReadOnly = true,
                                    StorageAccountName = storage.FileStorage.AccountName,
                                    StorageAccountKey = storage.FileStorage.AccessKey,
                                    ShareName = storage.FileStorage.ShareName
                                }
                            }
            },
            IpAddress = new IpAddressArgs
            {
                Type = ContainerGroupIpAddressType.Private,
                Ports = new PortArgs()
                {
                    Port = 80
                }
            },
            NetworkProfile = new ContainerGroupNetworkProfileArgs
            {
                Id = networkProfile.Value
            },
            OsType = "Linux",
            Containers = deployables.Select(deployable => new ContainerArgs
            {
                Name = deployable.Name.Value,
                Image = deployable.Image,
                Ports = new ContainerPortArgs[]
                    {
                            new()
                            {
                                Port = 80,
                                Protocol = "TCP"
                            }
                    },
                Resources = new ResourceRequirementsArgs()
                {
                    Requests = new ResourceRequestsArgs
                    {
                        Cpu = 1,
                        MemoryInGB = 1
                    }
                },
                VolumeMounts = new VolumeMountArgs()
                {
                    MountPath = "/app/config",
                    Name = "storage-config"
                }
            }).ToArray()
        });

        _ = new PrivateRecordSet(microservice.Name.Value, new()
        {
            ResourceGroupName = resourceGroup.Name,
            Ttl = 300,
            RelativeRecordSetName = microservice.Name.Value,
            PrivateZoneName = application.GetPrivateZoneName(),
            RecordType = "A",
            ARecords =
            {
                new ARecordArgs
                {
                    Ipv4Address = containerGroup.IpAddress.Apply(_ => _!.Ip!)
                }
            }
        });

        return containerGroup;
    }
}
