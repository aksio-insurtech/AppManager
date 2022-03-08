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
        var fileShare = new FileShare("ingress", new()
        {
            AccountName = storage.AccountName,
            ResourceGroupName = resourceGroup.Name,
        });

        var fileShareName = await fileShare.Name.GetValue();
        var content = TemplateTypes.IngressConfig(new { Something = 42 });
        var fileStorage = new FileStorage(storage.AccountName, storage.AccountKey, fileShareName, fileStorageLogger);
        fileStorage.Upload("nginx.conf", content);

        var containerGroup = new ContainerGroup("ingress", new()
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
                        StorageAccountName = storage.AccountName,
                        StorageAccountKey = storage.AccountKey,
                        ShareName = fileShare.Name
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
                    Command = "sh -c cp /app/config/nginx.conf /etc/nginx/nginx.conf && nginx -g daemon off;",
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
                }
            }
        });

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
                    Ipv4Address = containerGroup.IpAddress.Apply(_ => _!.Ip!)
                }
            }
        });
    }
}
