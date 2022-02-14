// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#pragma warning disable CS8019

using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Azure.Storage;
using Azure.Storage.Files.Shares;
using Common;
using Pulumi;
using Pulumi.Automation;
using Pulumi.AzureNative.ContainerInstance;
using Pulumi.AzureNative.ContainerInstance.Inputs;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using Pulumi.Mongodbatlas;
using Pulumi.Mongodbatlas.Inputs;
using FileShare = Pulumi.AzureNative.Storage.FileShare;

namespace Reactions.Applications
{
    public class PulumiStackDefinitions : IPulumiStackDefinitions
    {
        readonly IApplicationSettings _applicationSettings;

        public PulumiStackDefinitions(IApplicationSettings applicationSettings) => _applicationSettings = applicationSettings;

        public PulumiFn CreateApplication(Application application, RuntimeEnvironment environment) =>

            // Notes:
            // - Organization settings: Atlas OrgId
            // - Create MongoDB cluster for application
            // - Create MongoDB database user w/roll
            // - Add ClusterIP to allowed IP in MongoDB
            // - Automatically create dev & prod stacks
            // - Setup Kernel container instance
            // - Add tag for application for each stack
            // - Add tag for environment for each stack
            // - Store needed output values - show on resources tab
            // - Output
            PulumiFn.Create(async () =>
            {
                var tags = new Dictionary<string, string>
                    {
                        { "application", application.Id.ToString() }
                    };

                var resourceGroup = ResourceGroup.Get("Einar-D-Norway-RG", $"/subscriptions/{application.AzureSubscriptionId}/resourceGroups/Einar-D-Norway-RG");
                var storageAccount = new StorageAccount(application.Name.Value.ToLowerInvariant(), new StorageAccountArgs
                {
                    ResourceGroupName = resourceGroup.Name,
                    Tags = tags,
                    Sku = new SkuArgs
                    {
                        Name = SkuName.Standard_LRS
                    },
                    Kind = Kind.StorageV2
                });

                var fileShare = new FileShare("kernel", new()
                {
                    AccountName = storageAccount.Name,
                    ResourceGroupName = resourceGroup.Name
                });

                var getFileShareResult = GetFileShare.Invoke(new()
                {
                    AccountName = storageAccount.Name,
                    ResourceGroupName = resourceGroup.Name,
                    ShareName = "kernel"
                });

                var storageAccountKeysRequest = ListStorageAccountKeys.Invoke(new ListStorageAccountKeysInvokeArgs
                {
                    AccountName = storageAccount.Name,
                    ResourceGroupName = resourceGroup.Name
                });

                var storageAccountResult = GetStorageAccount.Invoke(new()
                {
                    AccountName = storageAccount.Name,
                    ResourceGroupName = resourceGroup.Name
                });

                var mongoDBOrganizationId = await _applicationSettings.GetMongoDBOrganizationId();

                var project = new Project(application.Name, new ProjectArgs
                {
                    OrgId = mongoDBOrganizationId.Value
                });

                var cluster = new Cluster("dev", new ClusterArgs
                {
                    ProjectId = project.Id,
                    ProviderName = "TENANT",
                    BackingProviderName = "AZURE",
                    ProviderInstanceSizeName = "M0",
                    ProviderRegionName = "EUROPE_NORTH"
                });

                var getClusterResult = GetCluster.Invoke(new()
                {
                    Name = cluster.Name,
                    ProjectId = cluster.ProjectId
                });

                var databasePassword = Guid.NewGuid().ToString();
                var user = new DatabaseUser("kernel", new()
                {
                    Username = "kernel",
                    ProjectId = cluster.ProjectId,
                    Password = databasePassword,
                    DatabaseName = "admin",
                    Roles = new DatabaseUserRoleArgs[]
                    {
                        new ()
                        {
                            DatabaseName = "admin",
                            RoleName = "readWriteAnyDatabase"
                        }
                    }
                });

                getFileShareResult.Apply(fs =>
                {
                    getClusterResult.Apply(clusterInfo =>
                        {
                            const string scheme = "mongodb+srv://";
                            var mongoDBConnectionString = clusterInfo.ConnectionStrings[0].StandardSrv.Insert(scheme.Length, $"kernel:{databasePassword}@");

                            Log.Info($"MongoDB ConnectionString : '{clusterInfo.ConnectionStrings[0].StandardSrv}'");
                            Log.Info($"MongoDB User 'kernel' has password '{databasePassword}'");
                            Log.Info($"MongoDB full connectionstring: '{mongoDBConnectionString}'");

                            storageAccountResult.Apply(account =>
                                {
                                    storageAccountKeysRequest.Apply(keys =>
                                    {
                                        var uri = new Uri(account.PrimaryEndpoints.File);
                                        var connectionString = $"DefaultEndpointsProtocol=https;AccountName={account.Name};AccountKey={keys.Keys[0].Value};EndpointSuffix=core.windows.net";

                                        Log.Info($"Storage Key : '{keys.Keys[0].Value}'");

                                        var share = new ShareClient(connectionString, "kernel");

                                        Log.Info("Get directory");
                                        var directory = share.GetDirectoryClient("./");

                                        Log.Info("Create file");
                                        var file = directory.GetFileClient("storage.json");

                                        var storageConfig = new Aksio.Cratis.Configuration.Storage();
                                        var readModels = storageConfig["readModels"] = new()
                                        {
                                            Type = "MongoDB",
                                        };
                                        readModels.Tenants["3352d47d-c154-4457-b3fb-8a2efb725113"] = $"{mongoDBConnectionString}/read-models";

                                        var eventStore = storageConfig["eventStore"] = new()
                                        {
                                            Type = "MongoDB",
                                        };
                                        eventStore.Tenants["3352d47d-c154-4457-b3fb-8a2efb725113"] = $"{mongoDBConnectionString}/event-store";

                                        var storageConfigJson = JsonSerializer.Serialize(storageConfig, new JsonSerializerOptions()
                                        {
                                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                                        });

                                        Log.Info("Upload file");
                                        var stream = new MemoryStream(Encoding.UTF8.GetBytes(storageConfigJson));
                                        file.DeleteIfExists();
                                        file.Create(stream.Length);
                                        file.Upload(stream);

                                        return keys;
                                    });

                                    return account;
                                });

                            return clusterInfo;
                        });

                    var container = new ContainerGroup("kernel", new()
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
                                StorageAccountKey = storageAccountKeysRequest.Apply(_ => _.Keys[0].Value),
                                StorageAccountName = storageAccount.Name,
                                ShareName = fileShare.Name
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
                                Image = "aksioinsurtech/cratis:5.6.0",
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

                    var projectIpAccessList = new ProjectIpAccessList("kernel", new()
                    {
                        ProjectId = project.Id,
                        IpAddress = container.IpAddress.Apply(_ => _!.Ip!)
                    });

                    return fs;
                });
            });

        public PulumiFn CreateDeployable(RuntimeEnvironment environment) => throw new NotImplementedException();
        public PulumiFn CreateMicroservice(RuntimeEnvironment environment) => throw new NotImplementedException();
    }
}
