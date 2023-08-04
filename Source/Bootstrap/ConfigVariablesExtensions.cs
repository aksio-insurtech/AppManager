// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Infrastructure;
using Reactions.Applications;

namespace Bootstrap;

public static class ConfigVariablesExtensions
{
    public static async Task<ApplicationAndEnvironment> ApplyConfigAndVariables(this ApplicationAndEnvironment applicationAndEnvironment, string basePath, ManagementConfig config)
    {
        var dockerHub = new DockerHub();

        applicationAndEnvironment = await ApplyLatestMiddlewareVersion(applicationAndEnvironment, dockerHub);
        applicationAndEnvironment = ApplyIdentityProviderValues(applicationAndEnvironment, config);
        applicationAndEnvironment = await ApplyGammaVersionIfGammaIsThere(applicationAndEnvironment, dockerHub);

        return applicationAndEnvironment with
        {
            Environment = applicationAndEnvironment.Environment with
            {
                AzureSubscriptionId = config.Azure.EnvironmentSubscriptionId,
                MongoDB = new(null!, new[] { new MongoDBUser("kernel", config.MongoDB.KernelUserPassword) })
            }
        };
    }

    static async Task<ApplicationAndEnvironment> ApplyGammaVersionIfGammaIsThere(ApplicationAndEnvironment applicationAndEnvironment, DockerHub dockerHub)
    {
        var gamma = applicationAndEnvironment.Environment.Microservices.FirstOrDefault(_ => _.Id == "8c538618-2862-4018-b29d-17a4ec131958");
        if (gamma is not null)
        {
            var appManagerVersion = await dockerHub.GetLastVersionOfAppManager();
            applicationAndEnvironment = applicationAndEnvironment with
            {
                Environment = applicationAndEnvironment.Environment with
                {
                    Microservices = new[]
                    {
                        applicationAndEnvironment.Environment.Microservices.First() with
                        {
                            Deployables = new[]
                            {
                                applicationAndEnvironment.Environment.Microservices.First().Deployables.First() with
                                {
                                    Image = $"docker.io/{DockerHubExtensions.AksioOrganization}/{DockerHubExtensions.AppManagerImage}:{appManagerVersion}"
                                }
                            }
                        }
                    }
                }
            };
        }

        return applicationAndEnvironment;
    }

    static async Task<ApplicationAndEnvironment> ApplyLatestMiddlewareVersion(ApplicationAndEnvironment applicationAndEnvironment, DockerHub dockerHub)
    {
        var ingressMiddlewareVersion = await dockerHub.GetLastVersionOfIngressMiddleware();

        return applicationAndEnvironment with
        {
            Environment = applicationAndEnvironment.Environment with
            {
                Ingresses = applicationAndEnvironment.Environment.Ingresses.Select(ingress => ingress with
                {
                    MiddlewareVersion = ingressMiddlewareVersion
                })
            }
        };
    }

    static ApplicationAndEnvironment ApplyIdentityProviderValues(ApplicationAndEnvironment applicationAndEnvironment, ManagementConfig config)
    {
        var ingresses = new List<Ingress>();
        foreach (var ingress in applicationAndEnvironment.Environment.Ingresses)
        {
            var providers = new List<IdentityProvider>();

            foreach (var identityProviderConfig in config.IdentityProviders)
            {
                var provider = ingress.IdentityProviders.FirstOrDefault(_ => _.Id == identityProviderConfig.Id);
                if (provider is not null)
                {
                    providers.Add(provider with
                    {
                        ClientId = identityProviderConfig.ClientId,
                        ClientSecret = identityProviderConfig.ClientSecret
                    });
                }
            }
            providers.AddRange(ingress.IdentityProviders.Where(_ => !providers.Any(p => p.Id == _.Id)));
            ingresses.Add(ingress with
            {
                IdentityProviders = providers
            });
        }

        return applicationAndEnvironment with
        {
            Environment = applicationAndEnvironment.Environment with
            {
                Ingresses = ingresses
            }
        };
    }
}
