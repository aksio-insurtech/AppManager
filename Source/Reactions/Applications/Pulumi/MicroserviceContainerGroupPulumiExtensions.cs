// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.ContainerInstance;
using Pulumi.AzureNative.ContainerInstance.Inputs;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class MicroserviceContainerGroupPulumiExtensions
{
    public static ContainerGroup SetupContainerGroup(this Microservice microservice, Application application, ResourceGroup resourceGroup, NetworkResult network, MicroserviceStorage storage, IEnumerable<Deployable> deployables, Tags tags)
    {
        var microserviceTags = tags.Clone();
        microserviceTags["microservice"] = microservice.Id.ToString();
        microserviceTags["microserviceName"] = microservice.Name.Value;

        return new ContainerGroup(microservice.Name, new()
        {
            ResourceGroupName = resourceGroup.Name,
            Volumes = new VolumeArgs[]
            {
                            new()
                            {
                                Name = "storage-config",
                                AzureFile = new AzureFileVolumeArgs()
                                {
                                    ReadOnly = true,
                                    StorageAccountName = storage.AccountName,
                                    StorageAccountKey = storage.AccessKey,
                                    ShareName = storage.ShareName
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
                Id = network.Profile.Id
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
    }
}
