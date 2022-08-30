// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Pulumi.AzureNative.App;
using Pulumi.AzureNative.App.Inputs;
using Pulumi.AzureNative.Resources;
using Reactions.Applications.Templates;
using FileShare = Pulumi.AzureNative.Storage.FileShare;

namespace Reactions.Applications.Pulumi;

#pragma warning disable RCS1175, IDE0059

public static class ApplicationIngressPulumiExtensions
{
    const string StorageName = "ingress-storage";

    public static async Task<string> SetupIngress(
        this Application application,
        ResourceGroup resourceGroup,
        StorageResult storage,
        ManagedEnvironment managedEnvironment,
        Tags tags,
        ILogger<FileStorage> fileStorageLogger,
        string? frontendMicroserviceResourceName = default)
    {
        var nginxFileShare = new FileShare("ingress", new()
        {
            AccountName = storage.AccountName,
            ResourceGroupName = resourceGroup.Name,
        });

        var nginxFileShareName = await nginxFileShare.Name.GetValue();
        var nginxFileStorage = new FileStorage(storage.AccountName, storage.AccountKey, nginxFileShareName, fileStorageLogger);

        var frontendUrl = string.Empty;
        if (frontendMicroserviceResourceName is not null)
        {
            var getMicroserviceContainerApp = GetContainerApp.Invoke(new()
            {
                ResourceGroupName = resourceGroup.Name,
                ContainerAppName = frontendMicroserviceResourceName
            });
            var microserviceContainerApp = await getMicroserviceContainerApp.GetValue();
            frontendUrl = $"http://{microserviceContainerApp.Configuration!.Ingress!.Fqdn}";
        }

        var nginxContent = TemplateTypes.IngressConfig(new
        {
            HasFrontend = frontendMicroserviceResourceName is not null,
            FrontendUrl = frontendUrl
        });
        nginxFileStorage.Upload("nginx.conf", nginxContent);

        var managedEnvironmentStorage = new ManagedEnvironmentsStorage(StorageName, new()
        {
            ResourceGroupName = resourceGroup.Name,
            EnvironmentName = managedEnvironment.Name,
            StorageName = StorageName,
            Properties = new ManagedEnvironmentStoragePropertiesArgs
            {
                AzureFile = new AzureFilePropertiesArgs
                {
                    AccessMode = "ReadOnly",
                    AccountKey = storage.AccountKey,
                    AccountName = storage.AccountName,
                    ShareName = nginxFileShareName
                }
            }
        });

        const string microsoftProviderAuthenticationSecret = "microsoft-provider-authentication-secret";
        var containerApp = new ContainerApp("ingress", new()
        {
            Location = resourceGroup.Location,
            Tags = tags,
            ResourceGroupName = resourceGroup.Name,
            ManagedEnvironmentId = managedEnvironment.Id,
            Configuration = new ConfigurationArgs
            {
                Ingress = new IngressArgs
                {
                    External = true,
                    TargetPort = 80
                },
                Secrets = new SecretArgs
                {
                    Name = microsoftProviderAuthenticationSecret,
                    Value = application.Authentication?.ClientSecret.Value ?? string.Empty
                }
            },
            Template = new TemplateArgs
            {
                Volumes = new VolumeArgs[]
                {
                    new()
                    {
                        Name = StorageName,
                        StorageName = StorageName,
                        StorageType = StorageType.AzureFile
                    }
                },
                Containers = new ContainerArgs
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
                    VolumeMounts = new VolumeMountArgs[]
                    {
                        new()
                        {
                            MountPath = "/config",
                            VolumeName = StorageName
                        }
                    }
                },
                Scale = new ScaleArgs
                {
                    MaxReplicas = 1,
                    MinReplicas = 1,
                }
            },
        });

        if (application.Authentication is not null)
        {
            var containerAppAut = new ContainerAppsAuthConfig("current", new()
            {
                AuthConfigName = "current",
                ResourceGroupName = resourceGroup.Name,
                ContainerAppName = containerApp.Name,
                GlobalValidation = new GlobalValidationArgs()
                {
                    UnauthenticatedClientAction = UnauthenticatedClientActionV2.RedirectToLoginPage,
                    RedirectToProvider = "Microsoft"
                },
                Platform = new AuthPlatformArgs
                {
                    Enabled = true
                },
                IdentityProviders = new IdentityProvidersArgs
                {
                    AzureActiveDirectory = new AzureActiveDirectoryArgs
                    {
                        Enabled = true,
                        IsAutoProvisioned = true,
                        Registration = new AzureActiveDirectoryRegistrationArgs
                        {
                            ClientId = application.Authentication.ClientId.Value,
                            ClientSecretSettingName = microsoftProviderAuthenticationSecret,
                            OpenIdIssuer = "https://login.microsoftonline.com/1042fa82-e1c7-40a8-9c61-a7831ef3f10a/v2.0"
                        },
                        Validation = new AzureActiveDirectoryValidationArgs
                        {
                            AllowedAudiences =
                            {
                                $"api://{application.Authentication.ClientId}"
                            }
                        }
                    }
                }
            });
        }

        var ingressConfig = await containerApp.Configuration.GetValue();
        return $"https://{ingressConfig!.Ingress!.Fqdn}";
    }
}
