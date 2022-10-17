// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Pulumi;
using Pulumi.AzureNative.App;
using Pulumi.AzureNative.App.Inputs;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class MicroserviceContainerAppPulumiExtensions
{
    public static Task<ContainerApp> GetContainerApp(this Microservice microservice, Application application, CloudRuntimeEnvironment environment)
    {
        var resourceGroup = application.GetResourceGroup(environment);
        var containerAppName = microservice.Name.Value.ToLowerInvariant();
        var getContainerApp = global::Pulumi.AzureNative.App.GetContainerApp.Invoke(new()
        {
            ContainerAppName = containerAppName,
            ResourceGroupName = resourceGroup.Name
        });
        var containerApp = ContainerApp.Get(containerAppName, getContainerApp.Apply(_ => _.Id));
        return Task.FromResult(containerApp);
    }

    public static async Task<ContainerAppResult> SetupContainerApp(
        this Microservice microservice,
        Application application,
        ResourceGroup resourceGroup,
        string managedEnvironmentId,
        string managedEnvironmentName,
        string containerRegistryLoginServer,
        string containerRegistryUsername,
        string containerRegistryPassword,
        MicroserviceStorage storage,
        IEnumerable<Deployable> deployables,
        Tags tags,
        bool useContainerRegistry = true)
    {
        var microserviceTags = tags.Clone();
        microserviceTags["MicroserviceId"] = microservice.Id.ToString();
        microserviceTags["Microservice"] = microservice.Name.Value;

        var storageName = $"{storage.FileStorage.ShareName}-storage";

        var managedEnvironmentStorage = new ManagedEnvironmentsStorage(microservice.Name, new()
        {
            ResourceGroupName = resourceGroup.Name,
            EnvironmentName = managedEnvironmentName,
            StorageName = storageName,
            Properties = new ManagedEnvironmentStoragePropertiesArgs
            {
                AzureFile = new AzureFilePropertiesArgs
                {
                    AccessMode = "ReadOnly",
                    AccountKey = storage.FileStorage.AccessKey,
                    AccountName = storage.FileStorage.AccountName,
                    ShareName = storage.FileStorage.ShareName
                }
            }
        });

        var containerApp = new ContainerApp(microservice.Name.Value.ToLowerInvariant(), new()
        {
            Location = resourceGroup.Location,
            Tags = microserviceTags,
            ResourceGroupName = resourceGroup.Name,
            ManagedEnvironmentId = managedEnvironmentId,
            Configuration = new ConfigurationArgs
            {
                Ingress = new IngressArgs
                {
                    External = false,
                    TargetPort = 80,
                    AllowInsecure = true
                },
                Secrets = !useContainerRegistry ? new InputList<SecretArgs>()
                    : new InputList<SecretArgs>()
                    {
                        new SecretArgs()
                        {
                            Name = "container-registry",
                            Value = containerRegistryPassword
                        }
                    },
                Registries = !useContainerRegistry ?
                    new InputList<RegistryCredentialsArgs>()
                    :
                    new InputList<RegistryCredentialsArgs>()
                    {
                        new RegistryCredentialsArgs()
                        {
                            Server = containerRegistryLoginServer,
                            Username = containerRegistryUsername,
                            PasswordSecretRef = "container-registry"
                        }
                    }
            },
            Template = new TemplateArgs
            {
                Volumes = new VolumeArgs[]
                {
                    new ()
                    {
                        Name = storageName,
                        StorageName = storageName,
                        StorageType = StorageType.AzureFile
                    }
                },
                Containers = deployables.Select(deployable => new ContainerArgs
                {
                    Name = deployable.Name.Value.ToLowerInvariant(),
                    Image = deployable.Image,

                    VolumeMounts = new VolumeMountArgs[]
                    {
                        new()
                        {
                            MountPath = "/app/config",
                            VolumeName = storageName
                        }
                    },

                    Resources = new ContainerResourcesArgs
                    {
                        Cpu = 1,
                        Memory = "2.0Gi"
                    }
                }).ToArray(),

                Scale = new ScaleArgs
                {
                    MaxReplicas = 1,
                    MinReplicas = 1,
                }
            },
        });

        var configuration = await containerApp.Configuration.GetValue();
        return new ContainerAppResult(containerApp, configuration!.Ingress!.Fqdn);
    }
}
