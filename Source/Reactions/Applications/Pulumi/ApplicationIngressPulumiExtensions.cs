// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Azure;
using Microsoft.Extensions.Logging;
using Pulumi.AzureNative.ContainerInstance;
using Pulumi.AzureNative.ContainerInstance.Inputs;
using Pulumi.AzureNative.Network;
using Pulumi.AzureNative.Network.Inputs;
using Pulumi.AzureNative.Resources;
using Reactions.Applications.Templates;
using FileShare = Pulumi.AzureNative.Storage.FileShare;

namespace Reactions.Applications.Pulumi;

public static class ApplicationIngressPulumiExtensions
{
    public static async Task SetupIngress(
        this Application application,
        ResourceGroup resourceGroup,
        AzureNetworkProfileIdentifier networkProfile,
        StorageResult storage,
        Tags tags,
        ILogger<FileStorage> fileStorageLogger)
    {
        var nginxFileShare = new FileShare("ingress-nginx", new()
        {
            AccountName = storage.AccountName,
            ResourceGroupName = resourceGroup.Name,
        });

        var nginxFileShareName = await nginxFileShare.Name.GetValue();
        var nginxFileStorage = new FileStorage(storage.AccountName, storage.AccountKey, nginxFileShareName, fileStorageLogger);
        var nginxContent = TemplateTypes.IngressConfig(new { Something = 42 });
        nginxFileStorage.Upload("nginx.conf", nginxContent);

        var containerGroup = new ContainerGroup("ingress", new()
        {
            Tags = tags,
            ResourceGroupName = resourceGroup.Name,
            Volumes = new VolumeArgs[]
            {
                new()
                {
                    Name = "nginx-config",
                    AzureFile = new AzureFileVolumeArgs()
                    {
                        ReadOnly = true,
                        StorageAccountName = storage.AccountName,
                        StorageAccountKey = storage.AccountKey,
                        ShareName = nginxFileShare.Name
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
            Containers =
            {
                new ContainerArgs
                {
                    Name = "nginx",
                    Image = "nginx",
                    Command =
                    {
                        "nginx",
                        "-c",
                        "/config/nginx.conf",
                        "-g",
                        "daemon off;"
                    },
                    Ports =
                    {
                        new ContainerPortArgs()
                        {
                            Port = 80,
                            Protocol = "TCP"
                        },
                        new ContainerPortArgs()
                        {
                            Port = 443,
                            Protocol = "TCP"
                        }
                    },
                    Resources = new ResourceRequirementsArgs()
                    {
                        Requests = new ResourceRequestsArgs
                        {
                            Cpu = 0.5,
                            MemoryInGB = 0.5
                        }
                    },
                    VolumeMounts =
                    {
                        new VolumeMountArgs()
                        {
                            MountPath = "/config",
                            Name = "nginx-config"
                        }
                    }
                }
            }
        });

        var getContainerGroupResult = GetContainerGroup.Invoke(new()
        {
            ContainerGroupName = containerGroup.Name,
            ResourceGroupName = resourceGroup.Name
        });
        var ipAddress = getContainerGroupResult.Apply(_ => _.IpAddress);

        _ = new PrivateRecordSet("ingress", new()
        {
            ResourceGroupName = resourceGroup.Name,
            Ttl = 300,
            RelativeRecordSetName = "ingress",
            PrivateZoneName = application.GetPrivateZoneName(),
            RecordType = "A",
            ARecords =
            {
                new ARecordArgs
                {
                    Ipv4Address = ipAddress.Apply(_ => _!.Ip!)
                }
            }
        });
    }
}
