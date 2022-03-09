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

        var vouchFileShare = new FileShare("ingress-vouch", new()
        {
            AccountName = storage.AccountName,
            ResourceGroupName = resourceGroup.Name,
        });

        var certbotConfigFileShare = new FileShare("ingress-certbot-config", new()
        {
            AccountName = storage.AccountName,
            ResourceGroupName = resourceGroup.Name,
        });

        var certbotWwwFileShare = new FileShare("ingress-certbot-www", new()
        {
            AccountName = storage.AccountName,
            ResourceGroupName = resourceGroup.Name,
        });

        var nginxFileShareName = await nginxFileShare.Name.GetValue();
        var nginxFileStorage = new FileStorage(storage.AccountName, storage.AccountKey, nginxFileShareName, fileStorageLogger);
        var nginxContent = TemplateTypes.IngressConfig(new { Something = 42 });
        nginxFileStorage.Upload("nginx.conf", nginxContent);

        var vouchFileShareName = await vouchFileShare.Name.GetValue();
        var vouchFileStorage = new FileStorage(storage.AccountName, storage.AccountKey, vouchFileShareName, fileStorageLogger);
        var vouchContent = TemplateTypes.VouchConfig(new { Something = 42 });
        vouchFileStorage.Upload("config.yml", vouchContent);

        var certbotConfigFileShareName = await certbotConfigFileShare.Name.GetValue();
        var certbotConfigFileStorage = new FileStorage(storage.AccountName, storage.AccountKey, certbotConfigFileShareName, fileStorageLogger);

        var certbotWwwFileShareName = await certbotWwwFileShare.Name.GetValue();
        var certbotWwwFileStorage = new FileStorage(storage.AccountName, storage.AccountKey, certbotWwwFileShareName, fileStorageLogger);

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
                },
                new()
                {
                    Name = "vouch-config",
                    AzureFile = new AzureFileVolumeArgs()
                    {
                        ReadOnly = true,
                        StorageAccountName = storage.AccountName,
                        StorageAccountKey = storage.AccountKey,
                        ShareName = vouchFileShare.Name
                    }
                },
                new()
                {
                    Name = "certbot-config",
                    AzureFile = new AzureFileVolumeArgs()
                    {
                        ReadOnly = true,
                        StorageAccountName = storage.AccountName,
                        StorageAccountKey = storage.AccountKey,
                        ShareName = certbotConfigFileShare.Name
                    }
                },
                new()
                {
                    Name = "certbot-www",
                    AzureFile = new AzureFileVolumeArgs()
                    {
                        ReadOnly = true,
                        StorageAccountName = storage.AccountName,
                        StorageAccountKey = storage.AccountKey,
                        ShareName = certbotWwwFileShare.Name
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
                        },
                        new VolumeMountArgs
                        {
                            MountPath = "/etc/letsencrypt",
                            Name = "certbot-config"
                        },
                        new VolumeMountArgs
                        {
                            MountPath = "/var/www/certbot",
                            Name = "certbot-www"
                        }
                    }
                },
                new ContainerArgs
                {
                    Name = "vouch",
                    Image = "quay.io/vouch/vouch-proxy",
                    Ports = new ContainerPortArgs[]
                        {
                                new()
                                {
                                    Port = 9090,
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
                    VolumeMounts = new VolumeMountArgs()
                    {
                        MountPath = "/config",
                        Name = "vouch-config"
                    }
                },
#if false
                new ContainerArgs
                {
                    Name = "certbot",
                    Image = "certbot/certbot",
                    Command =
                    {
                        "/bin/sh",
                        "-c",
                        "trap exit TERM; while :; do certbot renew; sleep 12h & wait $${!}; done;"
                    },
                    Resources = new ResourceRequirementsArgs()
                    {
                        Requests = new ResourceRequestsArgs
                        {
                            Cpu = 0.3,
                            MemoryInGB = 0.3
                        }
                    },
                    VolumeMounts =
                    {
                        new VolumeMountArgs
                        {
                            MountPath = "/etc/letsencrypt",
                            Name = "certbot-config"
                        },
                        new VolumeMountArgs
                        {
                            MountPath = "/var/www/certbot",
                            Name = "certbot-www"
                        }
                    }
                }
#endif
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
