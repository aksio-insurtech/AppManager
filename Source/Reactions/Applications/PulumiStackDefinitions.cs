// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Common;
using Concepts;
using Events.Applications;
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
        readonly ISettings _settings;
        readonly IExecutionContextManager _executionContextManager;
        readonly IEventLog _eventLog;

        public PulumiStackDefinitions(ISettings applicationSettings, IExecutionContextManager executionContextManager, IEventLog eventLog)
        {
            _settings = applicationSettings;
            _executionContextManager = executionContextManager;
            _eventLog = eventLog;
        }

        public PulumiFn CreateApplication(Application application, CloudRuntimeEnvironment environment) =>

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
                // Todo: Set to actual tenant - and probably not here!
                _executionContextManager.Establish(TenantId.Development, CorrelationId.New());

                var tags = new Dictionary<string, string>
                    {
                        { "application", application.Id.ToString() },
                        { "environment", Enum.GetName(typeof(CloudRuntimeEnvironment), environment) ?? string.Empty }
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

                var storageAccountKeysRequest = ListStorageAccountKeys.Invoke(new ListStorageAccountKeysInvokeArgs
                {
                    AccountName = storageAccount.Name,
                    ResourceGroupName = resourceGroup.Name
                });

                var mongoDBOrganizationId = _settings.MongoDBOrganizationId;

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

                var kernelDatabaseUserPassword = Guid.NewGuid().ToString();
                var user = new DatabaseUser("kernel", new()
                {
                    Username = "kernel",
                    ProjectId = cluster.ProjectId,
                    Password = kernelDatabaseUserPassword,
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

                var storage = new MicroserviceStorageShare(storageAccount.Name, storageAccountKeysRequest.Apply(_ => _.Keys[0].Value), fileShare.Name);
                await HandleKernelConfiguration(application, environment, resourceGroup.Name, cluster, kernelDatabaseUserPassword, storage);

                var container = MicroserviceWithDeployables(
                    resourceGroup.Name,
                    new(Guid.Empty, "kernel"),
                    storage,
                    new[]
                    {
                        new Deployable(Guid.Empty, "kernel", "aksioinsurtech/cratis:5.8.8")
                    });

                var projectIpAccessList = new ProjectIpAccessList("kernel", new()
                {
                    ProjectId = project.Id,
                    IpAddress = container.IpAddress.Apply(_ => _!.Ip!)
                });

                var ipAddress = await container.IpAddress.GetValue(_ => _!.Ip!);
                if (application.Resources?.IpAddress is null ||
                    application.Resources?.IpAddress?.Value != ipAddress)
                {
                    await _eventLog.Append(application.Id, new IpAddressSetForApplication(ipAddress));
                }
            });

        public PulumiFn CreateDeployable(CloudRuntimeEnvironment environment) => throw new NotImplementedException();
        public PulumiFn CreateMicroservice(CloudRuntimeEnvironment environment) => throw new NotImplementedException();

        ContainerGroup MicroserviceWithDeployables(Input<string> resourceGroupName, Microservice microservice, MicroserviceStorageShare storage, IEnumerable<Deployable> deployables)
        {
            return new ContainerGroup(microservice.Name, new()
            {
                ResourceGroupName = resourceGroupName,

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
                                    ShareName = storage.ShareName
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

        async Task HandleKernelConfiguration(Application application, CloudRuntimeEnvironment environment, Input<string> resourceGroupName, Cluster cluster, string databasePassword, MicroserviceStorageShare storage)
        {
            var getFileShareResult = GetFileShare.Invoke(new()
            {
                AccountName = storage.AccountName,
                ResourceGroupName = resourceGroupName,
                ShareName = storage.ShareName
            });

            var getClusterResult = GetCluster.Invoke(new()
            {
                Name = cluster.Name,
                ProjectId = cluster.ProjectId
            });

            var getStorageAccountResult = GetStorageAccount.Invoke(new()
            {
                AccountName = storage.AccountName,
                ResourceGroupName = resourceGroupName
            });

            var fileShareResult = await getFileShareResult.GetValue(_ => _);
            var clusterInfo = await getClusterResult.GetValue(_ => _);

            var connectionString = clusterInfo.ConnectionStrings[0].StandardSrv;
            if (application.Resources?.MongoDB?.ConnectionString is null ||
            application.Resources?.MongoDB?.ConnectionString.Value != connectionString)
            {
                await _eventLog.Append(application.Id, new MongoDBConnectionStringChangedForApplication(environment, connectionString));
            }

            const string scheme = "mongodb+srv://";
            var mongoDBConnectionString = connectionString.Insert(scheme.Length, $"kernel:{databasePassword}@");
            await _eventLog.Append(application.Id, new MongoDBUserChanged("kernel", databasePassword));

            var account = await getStorageAccountResult.GetValue();

            var key = await storage.AccountKey.GetValue();
            var kernelStorage = new KernelStorage(account.Name, key);
            kernelStorage.CreateAndUploadStorageJson(mongoDBConnectionString);
            kernelStorage.CreateAndUploadAppSettings(application, _settings);
        }
    }
}
