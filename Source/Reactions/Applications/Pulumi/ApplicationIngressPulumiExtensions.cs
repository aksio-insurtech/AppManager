// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.Cratis.Changes;
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
    const string AppSettingsFile = "appsettings.json";
    const string OpenIDConfigurationBlobName = "openid-configuration";

    /// <summary>
    /// Configure the ingress, returns true if any config files where in fact changed.
    /// </summary>
    /// <param name="application">Application.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="microservices">Microservices.</param>
    /// <param name="ingress">Ingress.</param>
    /// <param name="storage">Storage.</param>
    /// <param name="fileShareName">File share name.</param>
    /// <param name="fileStorageLogger">File storage logger.</param>
    /// <returns>True if any config files are changed, false if they are not changed.</returns>
    public static async Task<bool> ConfigureIngress(
        this Application application,
        ApplicationEnvironmentWithArtifacts environment,
        IDictionary<MicroserviceId, ContainerApp> microservices,
        Ingress ingress,
        Storage storage,
        string fileShareName,
        ILogger<FileStorage> fileStorageLogger)
    {
        var configChanged = false;

        var nginxFileStorage = new FileStorage(storage.AccountName, storage.AccountKey, fileShareName, fileStorageLogger);
        var routes = new List<IngressTemplateRouteContent>();
        var microserviceTargets = new Dictionary<MicroserviceId, string>();

        var targetMicroserviceForImpersonation = ingress.GetTargetMicroserviceIdForImpersonation(environment);
        var hasTargetMicroserviceForImpersonation = targetMicroserviceForImpersonation is not null &&
                                                    microservices.ContainsKey(targetMicroserviceForImpersonation);

        foreach (var route in ingress.Routes)
        {
            if (microservices.TryGetValue(route.TargetMicroservice, out var microservice))
            {
                var configuration = await microservice.Configuration!.GetValue();
                var targetUrl = $"http://{configuration!.Ingress!.Fqdn}";
                var url = $"{targetUrl}{route.TargetPath}";
                microserviceTargets[route.TargetMicroservice] = targetUrl;
                routes.Add(new(route.Path, url, route.UseResolver ? ingress.Resolver!.Value : null));
            }
        }

        var nginxContent = TemplateTypes.IngressConfig(
            new IngressTemplateContent(
                routes,
                hasTargetMicroserviceForImpersonation ? new(microserviceTargets[targetMicroserviceForImpersonation!]) : null));
        configChanged |= await nginxFileStorage.Upload(AuthConfigFile, nginxContent) == FileStorageResult.FileUploaded;

        var middlewareTemplate = await ingress.RenderMiddlewareTemplate(environment, microservices);
        configChanged |= await nginxFileStorage.Upload(MiddlewareConfigFile, middlewareTemplate) ==
                         FileStorageResult.FileUploaded;

        var appSettings = new JsonObject().ConfigureAppSettingsHint().ConfigureKestrel().ConfigureLogging();

        configChanged |= await nginxFileStorage.Upload(AppSettingsFile, appSettings) == FileStorageResult.FileUploaded;

        return configChanged;
    }

    public static async Task<string> RenderMiddlewareTemplate(
        this Ingress ingress,
        ApplicationEnvironmentWithArtifacts environment,
        IDictionary<MicroserviceId, ContainerApp> microservices)
    {
        var tenantConfigs = new List<TenantConfig>();
        var tenantResolutionConfigs = new List<TenantResolutionConfig>();

        if (ingress.RouteTenantResolution is not null || ingress.SpecifiedTenantResolution is not null)
        {
            object options = (ingress.RouteTenantResolution is not null)
                ? new RouteTenantResolutionOptions(ingress.RouteTenantResolution.RegularExpression)
                : new SpecifiedTenantResolutionOptions(ingress.SpecifiedTenantResolution!.TenantId);

            var optionsAsJsonString = JsonSerializer.Serialize(
                options,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            tenantResolutionConfigs.Add(new(ingress.RouteTenantResolution is not null ? "route" : "specified", optionsAsJsonString));
        }

        // A map of domain names from (currently only) idporten, and the source identifier they should resolve to.
        var domainResolutionHostnames = new Dictionary<string, string>();
        
        var idPortenConfig = OpenIDConnectConfig.Empty;
        var idPortenProvider = ingress.IdentityProviders.FirstOrDefault(_ => _.Type == IdentityProviderType.IdPorten);
        if (idPortenProvider is not null)
        {
            idPortenConfig = new(idPortenProvider.Issuer, idPortenProvider.AuthorizationEndpoint);
            tenantConfigs = environment.Tenants.Select(
                    tenant =>
                    {
                        var providerConfig = tenant.IdentityProviders.FirstOrDefault(_ => _.Id == idPortenProvider.Id);
                        var sourceIdentifiers = new List<string>();
                        if (!string.IsNullOrWhiteSpace(providerConfig?.Domain?.Name?.Value))
                        {
                            if (domainResolutionHostnames.ContainsKey(providerConfig.Domain.Name.Value))
                            {
                                throw new("Error: More than one idporten provider has the same domain name defined!");
                            }

                            var tenantSourceIdentifier = tenant.SourceIdentifiers?.FirstOrDefault() ?? string.Empty;
                            if (string.IsNullOrWhiteSpace(tenantSourceIdentifier))
                            {
                                throw new(
                                    "Error: a tenant with a idPorten provider with a domain name must have at least one sourceIdentifier configured!");
                            }

                            domainResolutionHostnames.Add(providerConfig.Domain.Name.Value, tenantSourceIdentifier);
                        }

                        return new TenantConfig(
                            tenant.Id.ToString(),
                            providerConfig?.Domain?.Name ?? string.Empty,
                            providerConfig?.OnBehalfOf ?? string.Empty,
                            sourceIdentifiers);
                    })
                .ToList();
        }

        var hasAadProvider = ingress.IdentityProviders.Any(_ => _.Type == IdentityProviderType.Azure); 
        foreach (var aadProvider in ingress.IdentityProviders.Where(_ => _.Type == IdentityProviderType.Azure))
        {
            // Add or update tenant configs.
            foreach (var tenant in environment.Tenants)
            {
                var providerConfig = tenant.IdentityProviders.FirstOrDefault(_ => _.Id == aadProvider.Id);
                var sourceIdentifiers = providerConfig?.SourceIdentifiers?.ToList() ?? new List<string>();
                if (!sourceIdentifiers.Any())
                {
                    continue;
                }

                var tenantConfig = tenantConfigs.SingleOrDefault(t => t.TenantId == tenant.Id.ToString());
                if (tenantConfig != null)
                {
                    tenantConfig.SourceIdentifiers.AddRange(sourceIdentifiers);
                }
                else
                {
                    tenantConfigs.Add(new(tenant.Id.ToString(), string.Empty, string.Empty, sourceIdentifiers));
                }
            }
        }

        if (idPortenProvider is not null || hasAadProvider)
        {
            tenantResolutionConfigs.Add(new("claim", "{}"));
        }

        // Add sourceIdentifiers that are set on the tenant level (outside identity providers).
        foreach (var tenant in environment.Tenants)
        {
            if (!(tenant.SourceIdentifiers?.Any() ?? false))
            {
                continue;
            }

            var tenantConfig = tenantConfigs.SingleOrDefault(t => t.TenantId == tenant.Id.ToString());
            if (tenantConfig != null)
            {
                tenantConfig.SourceIdentifiers.AddRange(tenant.SourceIdentifiers);
            }
            else
            {
                tenantConfigs.Add(new(tenant.Id.ToString(), string.Empty, string.Empty, tenant.SourceIdentifiers.ToList()));
            }
        }

        var identityDetailsUrl = string.Empty;
        if (ingress.IdentityDetailsProvider is not null && microservices.TryGetValue(
                ingress.IdentityDetailsProvider,
                out var identityDetailsProviderMicroservice))
        {
            var configuration = await identityDetailsProviderMicroservice.Configuration!.GetValue();
            identityDetailsUrl = $"http://{configuration!.Ingress!.Fqdn}/.aksio/me";
        }

        // Mutual tls does not require the tenant resolution strategy (as its not a relevant use-case at the moment).
        // If this is the case, it must be added as the last alternative as they are processed in order.
        if (!string.IsNullOrEmpty(ingress.MutualTLS?.AuthorityCertificate) && ingress.MutualTLS.AcceptedSerialNumbers.Any())
        {
            tenantResolutionConfigs.Add(new("none", "{}"));
        }

        if (!tenantResolutionConfigs.Any())
        {
#pragma warning disable CA2201, AS0008
            throw new("Code or configuration error! A tenant resolution must be defined for the ingress to work!");
#pragma warning restore CA2201, AS0008
        }

        // Finally add the hostname tenancy resolver, if we have any extra domain names that need resolving.
        if (domainResolutionHostnames.Any())
        {
            var hostCfg = new { Hostnames = domainResolutionHostnames };
            var hostConfigAsJson = JsonSerializer.Serialize(hostCfg, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            tenantResolutionConfigs.Insert(0, new("host", hostConfigAsJson));
        }

        // Cannot distinct a record with an array, so do it manually
        List<IngressMiddlewareAuthorizationConfig> authConfigs = new();
        foreach (var ip in ingress.IdentityProviders.SelectMany(ip => ip.IngressMiddlewareAuthorizationConfig()))
        {
            var existing = authConfigs.FirstOrDefault(c => c.ClientId == ip.ClientId);
            
            if (existing == default)
            {
                authConfigs.Add(ip);
                continue;
            }

            if (existing.NoAuthorizationRequired != ip.NoAuthorizationRequired)
            {
                // Diff, so add. This is likely to fail as the json becomes invalid.
                authConfigs.Add(ip);
                continue;
            }
            
            // Basic compare of the lists
            if (string.Join(",", existing.Roles.Order()) != string.Join(",", ip.Roles.Order()))
            {
                // Diff, so add. This is likely to fail as the json becomes invalid.
                authConfigs.Add(ip);
                continue;
            }
        }
        
        // Catch any obvious configuration errors here
        var duplicateIdentityProviders = authConfigs.GroupBy(_ => _.ClientId).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        if (duplicateIdentityProviders.Any())
        {
            throw new(
                $"Duplicate identity providers, you must correct config or code for this: {string.Join(", ", duplicateIdentityProviders)}");
        }
        
        var middlewareContent = new IngressMiddlewareTemplateContent(
            idPortenProvider is not null,
            idPortenConfig,
            identityDetailsUrl,
            tenantConfigs,
            tenantResolutionConfigs,
            ingress.OAuthBearerTokenProvider,
            ingress.GetImpersonationTemplateContent(environment),
            ingress.MutualTLS,
            authConfigs);

        return TemplateTypes.IngressMiddlewareConfig(middlewareContent);
    }

    public static async Task<IngressResult> SetupIngress(
        this Application application,
        ApplicationEnvironmentWithArtifacts environment,
        ResourceGroup resourceGroup,
        Storage storage,
        ManagedEnvironment managedEnvironment,
        IDictionary<CertificateId, Output<string>> certificates,
        Ingress ingress,
        Tags tags)
    {
        var ingressFileShareName = $"{ingress.Name}-ingress";
        var storageName = $"{ingress.Name}-ingress-storage";

        var nginxFileShare = new FileShare(
            ingressFileShareName,
            new()
            {
                AccountName = storage.AccountName,
                ResourceGroupName = resourceGroup.Name,
            });

        var managedEnvironmentStorage = new ManagedEnvironmentsStorage(
            storageName,
            new()
            {
                ResourceGroupName = resourceGroup.Name,
                EnvironmentName = managedEnvironment.Name,
                StorageName = storageName,
                Properties = new ManagedEnvironmentStoragePropertiesArgs
                {
                    AzureFile = new AzureFilePropertiesArgs
                    {
                        AccessMode = AccessMode.ReadOnly,
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
            managedEnvironmentStorage,
            domains,
            certificates);

        // Set Container App Authentication, if applicable.
        await SetupAuthenticationForIngress(environment, resourceGroup, containerApp, ingress, storage);

        var authIngressConfig = await containerApp.Configuration.GetValue();
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

        var tenantDomains = environment.Tenants.SelectMany(_ => _.IdentityProviders
            .Where(i => ingress.IdentityProviders.Any(idp => idp.Id == i.Id))
            .Select(i => i.Domain!))
            .Where(_ => _ is not null)
            .ToArray();

        if (tenantDomains?.Length > 0)
        {
            domains.AddRange(tenantDomains);
        }
        return domains;
    }

    static ContainerApp SetupIngress(
        ResourceGroup resourceGroup,
        ApplicationEnvironmentWithArtifacts environment,
        ManagedEnvironment managedEnvironment,
        Ingress ingress,
        Tags tags,
        ManagedEnvironmentsStorage storage,
        IEnumerable<Domain> domains,
        IDictionary<CertificateId, Output<string>> certificates) =>
        new(
            $"{ingress.Name}-ingress",
            new()
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
                        ClientCertificateMode = ingress.MutualTLS != null
                            ? IngressClientCertificateMode.Require
                            : IngressClientCertificateMode.Ignore,
                        CustomDomains = domains.Select(
                                domain => new CustomDomainArgs
                                {
                                    BindingType = BindingType.SniEnabled,

                                    // If the certificate is not available yet, pick a random one.. You then need to rerun this (TODO: in future code this needs to be automatically handled).
#pragma warning disable CS8604 // Possible null reference argument.
                                    CertificateId = domain.CertificateId != null ? certificates[domain.CertificateId] : certificates.Values.FirstOrDefault(),
#pragma warning restore CS8604 // Possible null reference argument.
                                    Name = domain.Name.Value
                                })
                            .ToArray(),
                        IpSecurityRestrictions = ingress.AccessList?.Select(
                                acl => new IpSecurityRestrictionRuleArgs()
                                { Action = "Allow", Name = acl.Name.Value, IpAddressRange = acl.Address.Value })
                            .ToList() ?? new()
                    },
                    Secrets = ingress.IdentityProviders.Select(
                            idp => new SecretArgs
                            {
                                Name = GetSecretNameForIdentityProvider(idp),
                                Value = idp.ClientSecret?.Value ?? string.Empty
                            })
                        .ToList()
                },
                Template = new TemplateArgs
                {
                    Volumes = new VolumeArgs[]
                    {
                        new()
                        {
                            Name = storage.Name,
                            StorageName = storage.Name,
                            StorageType = StorageType.AzureFile
                        }
                    },
                    Containers = new ContainerArgs[]
                    {
                        new()
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
                                    VolumeName = storage.Name
                                }
                            }
                        },
                        new()
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
                                    VolumeName = storage.Name
                                }
                            }
                        }
                    },
                    Scale = new ScaleArgs
                    {
                        MaxReplicas = 1,
                        MinReplicas = 1,
                    }
                }
            });

    static string GetSecretNameForIdentityProvider(IdentityProvider idp) => $"{idp.Name.Value.Replace(' ', '-')}-authentication-secret".ToLowerInvariant();

    static async Task SetupAuthenticationForIngress(
        ApplicationEnvironmentWithArtifacts environment,
        ResourceGroup resourceGroup,
        ContainerApp containerApp,
        Ingress ingress,
        Storage storage)
    {
        if (!ingress.IdentityProviders.Any())
        {
            return;
        }

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

        _ = new ContainerAppsAuthConfig(ingress.Name, new()
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
                    "/.aksio/id-porten/*",
                    "/.well-known/*"
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
                        OpenIdIssuer = identityProvider.Issuer.Value
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
                var proxyAuthorizationEndpoint = $"https://{ingress.AuthDomain!.Name}/.aksio/id-porten/authorize";
                using (var client = new HttpClient())
                {
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
                }
                break;

            case IdentityProviderType.OpenIDConnect:
                args.CustomOpenIdConnectProviders[identityProvider.Name.Value] = new CustomOpenIdConnectProviderArgs
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
                            WellKnownOpenIdConfiguration = $"{identityProvider.Issuer}/.well-known/openid-configuration"
                        }
                    }
                };
                break;
        }
    }
}
