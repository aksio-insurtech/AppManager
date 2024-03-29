// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Pulumi;
using Pulumi.AzureNative.App;
using Pulumi.AzureNative.App.Inputs;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class MicroserviceContainerAppPulumiExtensions
{
    public static Task<ContainerApp> GetContainerApp(this Microservice microservice, Application application, ApplicationEnvironmentWithArtifacts environment)
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
        ManagedEnvironment managedEnvironment,
        ContainerRegistryResult? containerRegistry,
        MicroserviceStorage storage,
        IEnumerable<Deployable> deployables,
        Tags tags,
        bool kernelResource = false)
    {
        var microserviceTags = tags.Clone();
        microserviceTags["MicroserviceId"] = microservice.Id.ToString();
        microserviceTags["Microservice"] = microservice.Name.Value;

        var storageName = $"{storage.FileStorage.ShareName}-storage";

        var managedEnvironmentStorage = new ManagedEnvironmentsStorage(microservice.Name, new()
        {
            ResourceGroupName = resourceGroup.Name,
            EnvironmentName = managedEnvironment.Name,
            StorageName = storageName,
            Properties = new ManagedEnvironmentStoragePropertiesArgs
            {
                AzureFile = new AzureFilePropertiesArgs
                {
                    AccessMode = AccessMode.ReadWrite,
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
            ManagedEnvironmentId = managedEnvironment.Id,
            Configuration = new ConfigurationArgs
            {
                Ingress = new IngressArgs
                {
                    External = false,
                    TargetPort = microservice.Port,
                    Transport = IngressTransportMethod.Http,
                    AllowInsecure = true
                },
                Secrets = containerRegistry is null ? new InputList<SecretArgs>()
                    : new InputList<SecretArgs>()
                    {
                        new SecretArgs()
                        {
                            Name = "container-registry",
                            Value = containerRegistry.Password
                        }
                    },
                Registries = containerRegistry is null ?
                    new InputList<RegistryCredentialsArgs>()
                    :
                    new InputList<RegistryCredentialsArgs>()
                    {
                        new RegistryCredentialsArgs()
                        {
                            Server = containerRegistry.LoginServer,
                            Username = containerRegistry.UserName,
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
                        Name = managedEnvironmentStorage.Name,
                        StorageName = managedEnvironmentStorage.Name,
                        StorageType = StorageType.AzureFile
                    }
                },
                Containers = deployables.Select(deployable => new ContainerArgs
                {
                    Name = deployable.Name.Value.ToLowerInvariant(),
                    Image = GetDeployableImageName(deployable, containerRegistry),

                    VolumeMounts = new VolumeMountArgs[]
                    {
                        new()
                        {
                            MountPath = deployable.MountPath?.Value ?? MountPath.Default.Value,
                            VolumeName = storageName
                        }
                    },

                    Resources = new ContainerResourcesArgs
                    {
                        Cpu = kernelResource ? 2 : 1,
                        Memory = kernelResource ? "4.0Gi" : "2.0Gi"
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

    static string GetDeployableImageName(Deployable deployable, ContainerRegistryResult? containerRegistry)
    {
        if (!deployable.Image.Contains('/') && containerRegistry is not null)
        {
            return $"{containerRegistry.LoginServer}/{deployable.Image}";
        }
        return deployable.Image;
    }
}
