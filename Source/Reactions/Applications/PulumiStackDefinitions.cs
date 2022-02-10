// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#pragma warning disable CS8019

using System.Text;
using Azure.Storage.Files.Shares;
using Pulumi;
using Pulumi.Automation;
using Pulumi.AzureNative.ContainerInstance;
using Pulumi.AzureNative.ContainerInstance.Inputs;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using Pulumi.Mongodbatlas;
using FileShare = Pulumi.AzureNative.Storage.FileShare;

namespace Reactions.Applications
{
    public class PulumiStackDefinitions : IPulumiStackDefinitions
    {
        public PulumiFn CreateApplication(Application application, RuntimeEnvironment environment) =>

            // Notes:
            // - Organization settings: Atlas OrgId
            // - Create MongoDB cluster for application
            // - Create MongoDB database user
            // - Automatically create dev & prod stacks
            // - Setup Kernel container instance
            // - Add tag for application for each stack
            // - Add tag for environment for each stack
            // - Store needed output values - show on resources tab
            // - Output
            PulumiFn.Create(() =>
            {
                var resourceGroup = ResourceGroup.Get("Einar-D-Norway-RG", $"/subscriptions/{application.AzureSubscriptionId}/resourceGroups/Einar-D-Norway-RG");
                var storageAccount = new StorageAccount(application.Name.Value.ToLowerInvariant(), new StorageAccountArgs
                {
                    ResourceGroupName = resourceGroup.Name,
                    Tags = new Dictionary<string, string>
                    {
                        { "application", application.Id.ToString() }
                    },
                    Sku = new SkuArgs
                    {
                        Name = SkuName.Standard_LRS
                    },
                    Kind = Kind.StorageV2
                });

                var storageAccountKeysRequest = ListStorageAccountKeys.Invoke(new ListStorageAccountKeysInvokeArgs
                {
                    AccountName = storageAccount.Name,
                    ResourceGroupName = resourceGroup.Name
                });
                var storageAccountKey = storageAccountKeysRequest.Apply(_ =>
                {
                    try
                    {
                        const string connectionString = "";

                        Console.WriteLine("Create share");
                        var share = new ShareClient(connectionString, "kernel");
                        share.DeleteIfExists();
                        share.Create();

                        Console.WriteLine("Create directory");
                        var directory = share.GetDirectoryClient("config");
                        directory.Create();

                        Console.WriteLine("Create file");
                        var file = directory.GetFileClient("storage.json");
                        var stream = new MemoryStream(Encoding.UTF8.GetBytes("{\"hello\": \"world\"}"));
                        file.Create(stream.Length);
                        file.Upload(stream);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    Console.WriteLine($"StorageAccountKey = {_.Keys[0].Value}");
                    return _.Keys[0].Value;
                });

                var blobContainer = new BlobContainer("kernel", new BlobContainerArgs
                {
                    AccountName = storageAccount.Name,
                    ResourceGroupName = resourceGroup.Name,
                    ContainerName = "kernel",
                    PublicAccess = PublicAccess.None
                });

                var storageJson = new Blob("storage.json", new BlobArgs
                {
                    AccountName = storageAccount.Name,
                    ResourceGroupName = resourceGroup.Name,
                    ContainerName = "kernel",
                    BlobName = "storage.json",
                    ContentType = "application/json",
                    Source = new StringAsset("{\"hello\":\"world\"}")
                });

#if false
                var project = new Project(application.Name, new ProjectArgs
                {
                    OrgId = "61f150d98bc1f86a0748984d"
                });

                var cluster = new Cluster("dev", new ClusterArgs
                {
                    ProjectId = project.Id,
                    ProviderName = "TENANT",
                    BackingProviderName = "AZURE",
                    ProviderInstanceSizeName = "M0",
                    ProviderRegionName = "EUROPE_NORTH"
                });


                var container = new ContainerGroup("kernel", new()
                {
                    ResourceGroupName = resourceGroup.Name,
                    Volumes = new VolumeArgs[]
                    {
                        new()
                        {
                            Name = "storageConfig",
                            AzureFile = new AzureFileVolumeArgs()
                            {
                                ReadOnly = true,
                                StorageAccountKey = storageAccountKey,
                                StorageAccountName = storageAccount.Name,
                                ShareName = "kernel/storage.json"
                            }
                        }
                    },
                    IpAddress = new IpAddressArgs
                    {
                        Type = ContainerGroupIpAddressType.Public,
                        Ports = new PortArgs()
                        {
                            Port = 80
                        }
                    },
                    OsType = "Linux",
                    Containers = new ContainerArgs[]
                    {
                        new()
                        {
                            Name = "kernel",
                            Image = "aksioinsurtech/cratis:latest",
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
                                MountPath = "/app/storage.json",
                                Name = "storageConfig"
                            }
                        }
                    }
                });
#endif
            });

        public PulumiFn CreateDeployable(RuntimeEnvironment environment) => throw new NotImplementedException();
        public PulumiFn CreateMicroservice(RuntimeEnvironment environment) => throw new NotImplementedException();
    }
}
