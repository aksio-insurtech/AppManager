// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Nodes;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
    const string AuthConfigFile = "nginx.conf";
    const string MiddlewareConfigFile = "config.json";
    const string OpenIDConfigurationBlobName = "openid-configuration";

    public static async Task ConfigureIngress(
        this Application application,
        ApplicationEnvironmentWithArtifacts environment,
        IDictionary<MicroserviceId, ContainerApp> microservices,
        Ingress ingress,
        Storage storage,
        string fileShareName,
        ILogger<FileStorage> fileStorageLogger)
    {
        var nginxFileStorage = new FileStorage(storage.AccountName, storage.AccountKey, fileShareName, fileStorageLogger);
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

        var nginxContent = TemplateTypes.IngressConfig(new IngressTemplateContent(routes));
        nginxFileStorage.Upload(AuthConfigFile, nginxContent);

        var idPortenConfig = OpenIDConnectConfig.Empty;
        var idPortenProvider = ingress.IdentityProviders.FirstOrDefault(_ => _.Type == IdentityProviderType.IdPorten);
        if (idPortenProvider is not null)
        {
            var proxyAuthorizationEndpoint = $"https://{ingress.AuthDomain}/.aksio/id-porten/authorize";
            idPortenConfig = new(
                idPortenProvider.Issuer,
                idPortenProvider.AuthorizationEndpoint,
                proxyAuthorizationEndpoint);
        }

        var middlewareContent = new IngressMiddlewareTemplateContent(
            idPortenProvider is not null,
            idPortenConfig,
            environment.Tenants.Select(tenant => new TenantConfig(tenant.Id.ToString(), tenant.Domain?.Name ?? string.Empty, tenant.OnBehalfOf ?? string.Empty)));
        var middlewareTemplate = TemplateTypes.IngressMiddlewareConfig(middlewareContent);
        nginxFileStorage.Upload(MiddlewareConfigFile, middlewareTemplate);
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

        var fileShareId = await nginxFileShare.Id.GetValue();

        var domains = GetAllDomains(environment, ingress);

        var containerApp = SetupIngress(
            resourceGroup,
            environment,
            managedEnvironment,
            ingress,
            tags,
            storageName,
            domains,
            certificateResourceIdentifiers);
        await SetupAuthenticationForIngress(environment, resourceGroup, containerApp, ingress, storage);
        var authIngressConfig = await containerApp.Configuration.GetValue();
        await application.ConfigureIngress(
            environment,
            microservices,
            ingress,
            storage,
            ingressFileShareName,
            fileStorageLogger);

        return new($"https://{authIngressConfig!.Ingress!.Fqdn}", fileShareId, ingressFileShareName, containerApp);
    }

    static IEnumerable<Domain> GetAllDomains(ApplicationEnvironmentWithArtifacts environment, Ingress ingress)
    {
        var domains = new List<Domain>();
        if (ingress.Domain is not null)
        {
            domains.Add(ingress.Domain);
        }
        if (ingress.AuthDomain is not null)
        {
            domains.Add(ingress.AuthDomain);
        }
        domains.AddRange(environment.Tenants.Select(_ => _.Domain!).Where(_ => _ is not null));
        return domains;
    }

    static ContainerApp SetupIngress(
        ResourceGroup resourceGroup,
        ApplicationEnvironmentWithArtifacts environment,
        ManagedEnvironment managedEnvironment,
        Ingress ingress,
        Tags tags,
        string storageName,
        IEnumerable<Domain> domains,
        IDictionary<CertificateId, Output<string>> certificates) =>
        new($"{ingress.Name}-ingress", new()
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
                    Transport = IngressTransportMethod.Http,
                    CustomDomains = domains.Select(domain => new CustomDomainArgs
                    {
                        BindingType = BindingType.SniEnabled,
                        CertificateId = certificates[domain.CertificateId],
                        Name = domain.Name.Value
                    }).ToArray()
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

    static string GetSecretNameForIdentityProvider(IdentityProvider idp) => $"{idp.Name.Value.Replace(' ', '-')}-authentication-secret".ToLowerInvariant();

    static async Task SetupAuthenticationForIngress(
        ApplicationEnvironmentWithArtifacts environment,
        ResourceGroup resourceGroup,
        ContainerApp containerApp,
        Ingress ingress,
        Storage storage)
    {
        var identityProviderArgs = new IdentityProvidersArgs
        {
            CustomOpenIdConnectProviders = new Dictionary<string, CustomOpenIdConnectProviderArgs>()
        };

        foreach (var identityProvider in ingress.IdentityProviders)
        {
            await ConfigureIdentityProvider(ingress, identityProviderArgs, identityProvider, storage);
        }

        var redirectToProvider = "Microsoft";
        var firstProvider = ingress.IdentityProviders.FirstOrDefault();
        if (firstProvider?.Type == IdentityProviderType.IdPorten)
        {
            redirectToProvider = firstProvider.Name;
        }

        _ = new ContainerAppsAuthConfig("current", new()
        {
            AuthConfigName = "current",
            ResourceGroupName = resourceGroup.Name,
            ContainerAppName = containerApp.Name,
            GlobalValidation = new GlobalValidationArgs()
            {
                UnauthenticatedClientAction = UnauthenticatedClientActionV2.RedirectToLoginPage,
                RedirectToProvider = redirectToProvider,
                ExcludedPaths = new[]
                {
                    "/.aksio/*",
                    "/.aksio/id-porten/.well-known/openid-configuration"
                }
            },
            Platform = new AuthPlatformArgs
            {
                Enabled = true
            },
            IdentityProviders = identityProviderArgs
        });
    }

    static async Task ConfigureIdentityProvider(
        Ingress ingress,
        IdentityProvidersArgs args,
        IdentityProvider identityProvider,
        Storage storage)
    {
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
                var proxyAuthorizationEndpoint = $"https://{ingress.AuthDomain}/.aksio/id-porten/authorize";
                var client = new HttpClient();
                var url = $"{identityProvider.Issuer}/.well-known/openid-configuration";
                var result = await client.GetAsync(url);
                var json = await result.Content.ReadAsStringAsync();

                var document = (JsonNode.Parse(json) as JsonObject)!;
                document["authorization_endpoint"] = proxyAuthorizationEndpoint;

                var blobContainerClient = new BlobContainerClient(storage.ConnectionString, $"{ingress.Name}-ingress");
                await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                var blobClient = blobContainerClient.GetBlobClient(OpenIDConfigurationBlobName);
                await blobClient.DeleteIfExistsAsync();
                await blobContainerClient.UploadBlobAsync(OpenIDConfigurationBlobName, new BinaryData(document.ToJsonString()));

                args.CustomOpenIdConnectProviders[identityProvider.Name.Value] =
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
                                WellKnownOpenIdConfiguration = blobClient.Uri.ToString()
                            }
                        }
                    };
                break;
        }
    }
}
