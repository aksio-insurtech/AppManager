// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Concepts.Applications.Environments.Ingresses.IdentityProviders;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Pulumi;
using Pulumi.AzureNative.App;
using Pulumi.AzureNative.App.Inputs;
using Pulumi.AzureNative.Resources;
using Reactions.Applications.Templates;
using FileShare = Pulumi.AzureNative.Storage.FileShare;

namespace Reactions.Applications.Pulumi;

#pragma warning disable RCS1175

public static class ApplicationIngressPulumiExtensions
{
    const string AuthConfigFile = "auth-nginx.conf";
    const string MiddlewareConfigFile = "config.json";
    const string CompositionConfigFile = "composition-nginx.conf";

    public static async Task ConfigureAuthIngress(
        this Application application,
        ApplicationEnvironmentWithArtifacts environment,
        ResourceGroup resourceGroup,
        Ingress ingress,
        Storage storage,
        FileShare fileShare,
        AuthIngressTemplateContent templateContent,
        ILogger<FileStorage> fileStorageLogger)
    {
        var nginxFileShareName = await fileShare.Name.GetValue();
        var nginxFileStorage = new FileStorage(storage.AccountName, storage.AccountKey, nginxFileShareName, fileStorageLogger);
        var nginxContent = TemplateTypes.AuthIngressConfig(templateContent);
        nginxFileStorage.Upload(AuthConfigFile, nginxContent);

        var middlewareContent = new IngressMiddlewareTemplateContent(
            true,
            false,
            new AzureAdConfig("https://login.microsoftonline.com/1042fa82-e1c7-40a8-9c61-a7831ef3f10a/v2.0", $"https://{ingress.AuthDomain}/.aksio/azuread"),
            IdPortenConfig.Empty,
            environment.Tenants.Select(tenant => new TenantConfig(tenant.Id.ToString(), tenant.Domain, tenant.OnBehalfOf)));
        var middlewareTemplate = TemplateTypes.IngressMiddlewareConfig(middlewareContent);
        nginxFileStorage.Upload(MiddlewareConfigFile, middlewareTemplate);
    }

    public static async Task ConfigureCompositionIngress(
        this Application application,
        ResourceGroup resourceGroup,
        Ingress ingress,
        Storage storage,
        FileShare fileShare,
        IDictionary<MicroserviceId, ContainerApp> microservices,
        ILogger<FileStorage> fileStorageLogger)
    {
        var nginxFileShareName = await fileShare.Name.GetValue();
        var nginxFileStorage = new FileStorage(storage.AccountName, storage.AccountKey, nginxFileShareName, fileStorageLogger);

        var routes = new List<IngressTemplateRouteContent>();

        foreach (var route in ingress.Routes)
        {
            if (microservices.ContainsKey(route.TargetMicroservice))
            {
                var configuration = await microservices[route.TargetMicroservice].Configuration!.GetValue();
                var url = $"http://{configuration!.Ingress!.Fqdn}{route.TargetPath}";
                routes.Add(new IngressTemplateRouteContent(route.Path, url));
            }
        }

        var nginxContent = TemplateTypes.CompositionIngressConfig(new CompositionIngressTemplateContent(routes));
        nginxFileStorage.Upload(CompositionConfigFile, nginxContent);
    }

    public static async Task<IngressResult> SetupIngress(
        this Application application,
        ApplicationEnvironmentWithArtifacts environment,
        ResourceGroup resourceGroup,
        Storage storage,
        ManagedEnvironment managedEnvironment,
        Ingress ingress,
        IDictionary<MicroserviceId, ContainerApp> microservices,
        Tags tags,
        ILogger<FileStorage> fileStorageLogger)
    {
        var ingressFileShareName = $"{ingress.Name}-ingress";
        var storageName = $"{ingress.Name}-ingress-storage";

        var nginxFileShare = new FileShare(ingressFileShareName, new()
        {
            AccountName = storage.AccountName,
            ResourceGroupName = resourceGroup.Name,
        });

        var certificateResourceIdentifiers = new Dictionary<CertificateId, Output<string>>();
        foreach (var certificate in environment.Certificates)
        {
            var certificateResult = new global::Pulumi.AzureNative.App.Certificate(certificate.Name, new()
            {
                ResourceGroupName = resourceGroup.Name,
                Tags = tags,
                EnvironmentName = managedEnvironment.Name,
                Properties = new CertificatePropertiesArgs()
                {
                    Value = certificate.Value.Value,
                    Password = certificate.Password.Value
                }
            });
            certificateResourceIdentifiers[certificate.Id] = certificateResult.Id;
        }

        await application.ConfigureCompositionIngress(resourceGroup, ingress, storage, nginxFileShare, microservices, fileStorageLogger);

        _ = new ManagedEnvironmentsStorage(storageName, new()
        {
            ResourceGroupName = resourceGroup.Name,
            EnvironmentName = managedEnvironment.Name,
            StorageName = storageName,
            Properties = new ManagedEnvironmentStoragePropertiesArgs
            {
                AzureFile = new AzureFilePropertiesArgs
                {
                    AccessMode = "ReadOnly",
                    AccountKey = storage.AccountKey,
                    AccountName = storage.AccountName,
                    ShareName = nginxFileShare.Name
                }
            }
        });

        var compositionContainerApp = SetupCompositionIngress(resourceGroup, managedEnvironment, ingress, tags, storageName);

        var compositionIngressConfig = await compositionContainerApp.Configuration.GetValue();

        var fileShareId = await nginxFileShare.Id.GetValue();
        SetupAuthenticationForIngress(environment, resourceGroup, compositionContainerApp, ingress);

        var authContainerApp = SetupAuthIngress(resourceGroup, environment, managedEnvironment, ingress, tags, storageName, certificateResourceIdentifiers);
        var authIngressConfig = await authContainerApp.Configuration.GetValue();
        await application.ConfigureAuthIngress(
            environment,
            resourceGroup,
            ingress,
            storage,
            nginxFileShare,
            new AuthIngressTemplateContent($"http://{authIngressConfig!.Ingress!.Fqdn}", $"http://{compositionIngressConfig!.Ingress!.Fqdn}"),
            fileStorageLogger);

        return new($"https://{authIngressConfig!.Ingress!.Fqdn}", fileShareId, ingressFileShareName, authContainerApp, compositionContainerApp);
    }

    static ContainerApp SetupAuthIngress(
        ResourceGroup resourceGroup,
        ApplicationEnvironmentWithArtifacts environment,
        ManagedEnvironment managedEnvironment,
        Ingress ingress,
        Tags tags,
        string storageName,
        IDictionary<CertificateId, Output<string>> certificates) =>
        new($"{ingress.Name}-ingress-auth", new()
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
                    TargetPort = 80,
                    CustomDomains = environment.Tenants.Select(tenant => new CustomDomainArgs
                    {
                        BindingType = BindingType.SniEnabled,
                        CertificateId = certificates[tenant.CertificateId],
                        Name = tenant.Domain.Value
                    }).Concat(new[]
                    {
                        new CustomDomainArgs
                        {
                            BindingType = BindingType.SniEnabled,
                            CertificateId = certificates[ingress.AuthCertificateId],
                            Name = ingress.AuthDomain.Value
                        }
                    }).ToArray()
                },
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
                Containers = new ContainerArgs[]
                {
                    new ContainerArgs
                    {
                        Name = "nginx",
                        Image = "nginx",
                        Command =
                            {
                                "nginx",
                                "-c",
                                $"/config/{AuthConfigFile}",
                                "-g",
                                "daemon off;"
                            },
                        VolumeMounts = new VolumeMountArgs[]
                                {
                                    new()
                                    {
                                        MountPath = "/config",
                                        VolumeName = storageName
                                    }
                                }
                    },
                    new ContainerArgs
                    {
                        Name = "middleware",
                        Image = $"{DockerHubExtensions.AksioOrganization}/{DockerHubExtensions.IngressMiddlewareImage}:{ingress.MiddlewareVersion}",
                        Command =
                            {
                                "./IngressMiddleware",
                                "--urls",
                                "http://*:81"
                            },

                        VolumeMounts = new VolumeMountArgs[]
                                {
                                    new()
                                    {
                                        MountPath = "/app/config",
                                        VolumeName = storageName
                                    }
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

    static ContainerApp SetupCompositionIngress(ResourceGroup resourceGroup, ManagedEnvironment managedEnvironment, Ingress ingress, Tags tags, string storageName) =>
        new($"{ingress.Name}-ingress-composition", new()
        {
            Location = resourceGroup.Location,
            Tags = tags,
            ResourceGroupName = resourceGroup.Name,
            ManagedEnvironmentId = managedEnvironment.Id,
            Configuration = new ConfigurationArgs
            {
                Ingress = new IngressArgs
                {
                    External = false,
                    TargetPort = 80,
                    AllowInsecure = true
                },
                Secrets = ingress.IdentityProviders.Select(idp => new SecretArgs
                {
                    Name = GetSecretNameForIdentityProvider(idp),
                    Value = idp.ClientSecret?.Value ?? string.Empty
                }).ToList()
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
                Containers = new ContainerArgs
                {
                    Name = "nginx",
                    Image = "nginx",
                    Command =
                        {
                            "nginx",
                            "-c",
                            $"/config/{CompositionConfigFile}",
                            "-g",
                            "daemon off;"
                        },
                    VolumeMounts = new VolumeMountArgs[]
                            {
                            new()
                            {
                                MountPath = "/config",
                                VolumeName = storageName
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

    static string GetSecretNameForIdentityProvider(IdentityProvider idp) => $"{idp.Name.Value.Replace(' ', '-')}-authentication-secret".ToLowerInvariant();

    static void SetupAuthenticationForIngress(ApplicationEnvironmentWithArtifacts environment, ResourceGroup resourceGroup, ContainerApp containerApp, Ingress ingress)
    {
        foreach (var identityProvider in ingress.IdentityProviders)
        {
            _ = new ContainerAppsAuthConfig("current", new()
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
                IdentityProviders = GetIdentityPropertyConfiguration(identityProvider)
            });
        }
    }

    static IdentityProvidersArgs GetIdentityPropertyConfiguration(IdentityProvider identityProvider)
    {
        var args = new IdentityProvidersArgs();

        switch (identityProvider.Type)
        {
            case IdentityProviderType.Azure:
                args.AzureActiveDirectory = new AzureActiveDirectoryArgs
                {
                    Enabled = true,
                    IsAutoProvisioned = true,
                    Registration = new AzureActiveDirectoryRegistrationArgs
                    {
                        ClientId = identityProvider.ClientId.Value,
                        ClientSecretSettingName = GetSecretNameForIdentityProvider(identityProvider),
                        OpenIdIssuer = "https://login.microsoftonline.com/1042fa82-e1c7-40a8-9c61-a7831ef3f10a/v2.0"
                    },
                    Validation = new AzureActiveDirectoryValidationArgs
                    {
                        AllowedAudiences =
                                {
                                    $"api://{identityProvider.ClientId}"
                                }
                    }
                };
                break;

            case IdentityProviderType.IdPorten:
                args.CustomOpenIdConnectProviders = new Dictionary<string, CustomOpenIdConnectProviderArgs>
                {
                    {
                        identityProvider.Name.Value,
                        new CustomOpenIdConnectProviderArgs
                        {
                            Enabled = true,
                            Login = new OpenIdConnectLoginArgs
                            {
                                Scopes = new string[]
                                {
                                    "openid",
                                    "profile"
                                }
                            },
                            Registration = new OpenIdConnectRegistrationArgs
                            {
                                ClientId = identityProvider.ClientId.Value,
                                ClientCredential = new OpenIdConnectClientCredentialArgs
                                {
                                    ClientSecretSettingName = GetSecretNameForIdentityProvider(identityProvider),
                                    Method = ClientCredentialMethod.ClientSecretPost
                                },
                                OpenIdConnectConfiguration = new OpenIdConnectConfigArgs
                                {
                                    Issuer = identityProvider.Issuer.Value,
                                    AuthorizationEndpoint = identityProvider.AuthorizationEndpoint.Value,
                                    TokenEndpoint = identityProvider.TokenEndpoint.Value,
                                    CertificationUri = identityProvider.CertificationUri.Value
                                }
                            }
                        }
                    }
                };
                break;
        }

        return args;
    }
}
