// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Aksio.Cratis.Execution;
using Aksio.Cratis.Json;
using Concepts;
using Concepts.Azure;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Reactions.Applications;
using Reactions.Applications.Pulumi;
using Read.Settings;

namespace Bootstrap;

public static class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Missing config file as parameter");
            return;
        }

        var loggerFactory = LoggerFactory.Create(_ => _.AddConsole());
        var serializerOptions = Globals.JsonSerializerOptions;
        serializerOptions.Converters.Add(new SemanticVersionJsonConverter());

        // Setup application within cloud environment
        // Wait till its ready and then append the events that represents the actions done (through commands?)
        var configAsJson = await File.ReadAllTextAsync(args[0]);
        var config = JsonSerializer.Deserialize<ManagementConfig>(configAsJson, serializerOptions)!;

        var settings = new Settings(
            new AzureSubscription[] { config.Azure.Subscription },
            config.Pulumi.Organization,
            config.Pulumi.AccessToken,
            config.MongoDB.OrganizationId,
            config.MongoDB.PublicKey,
            config.MongoDB.PrivateKey,
            config.Azure.ServicePrincipal);

        var applicationAndEnvironmentAsJson = await File.ReadAllTextAsync("./AppManager.json");
        var applicationAndEnvironment = JsonSerializer.Deserialize<ApplicationAndEnvironment>(applicationAndEnvironmentAsJson, serializerOptions)!;
        applicationAndEnvironment = await ApplyConfigAndVariables(applicationAndEnvironment, config);
        var application = new Application(applicationAndEnvironment.Id, applicationAndEnvironment.Name);

        var executionContextManager = new ExecutionContextManager();
        var eventLog = new InMemoryEventLog();
        var executionContext = new ExecutionContext(MicroserviceId.Unspecified, TenantId.Development, CorrelationId.New(), CausationId.System, CausedBy.System);
        executionContextManager.Set(executionContext);

        var logger = loggerFactory.CreateLogger<FileStorage>();
        var definitions = new PulumiStackDefinitions(settings, executionContextManager, eventLog, logger);

        var stacksForApplications = new BootstrapStacksForApplications();
        var stacksForMicroservices = new BootstrapStacksForMicroservices(application.Id);
        var operations = new PulumiOperations(
            settings,
            executionContextManager,
            definitions,
            stacksForApplications,
            stacksForMicroservices,
            loggerFactory.CreateLogger<PulumiOperations>(),
            loggerFactory.CreateLogger<FileStorage>());

        await operations.ConsolidateEnvironment(application, applicationAndEnvironment.Environment);

#if false
        IngressResult? ingressResult = default;
        ApplicationEnvironmentResult? applicationEnvironmentResult = default;

        await operations.Up(
            application,
            PulumiFn.Create(async () =>
            {
                applicationEnvironmentResult = await definitions.ApplicationEnvironment(executionContext, application, development, cratisVersion);
                development = await applicationEnvironmentResult.MergeWithApplicationEnvironment(development);
                ingressResult = await definitions.Ingress(executionContext, application, development, ingress, applicationEnvironmentResult.ResourceGroup);

                var appManagerApi = new AppManagerApi(config, ingressResult.Url, _jsonSerializerOptions);
                await appManagerApi.Authenticate();

                stacksForApplications.AppManagerApi = appManagerApi;
                stacksForMicroservices.AppManagerApi = appManagerApi;
            }),
            development);

        await operations.Up(
            application,
            PulumiFn.Create(async () =>
            {
                if (applicationEnvironmentResult is null)
                {
                    return;
                }

                var resourceGroup = application.GetResourceGroup(development);

                var storage = await application.GetStorage(development, applicationEnvironmentResult!.ResourceGroup);
                var microserviceResult = await definitions.Microservice(
                    executionContext,
                    application,
                    microservice,
                    development,
                    false,
                    resourceGroup: resourceGroup,
                    deployables: microservice.Deployables);

                var fileShare = Pulumi.AzureNative.Storage.FileShare.Get(ingressResult!.FileShareName, ingressResult!.FileShareId);
                await application.ConfigureIngress(
                    resourceGroup,
                    ingress,
                    storage,
                    fileShare,
                    new Dictionary<MicroserviceId, ContainerApp>()
                    {
                        { microservice.Id, microserviceResult.ContainerApp }
                    },
                    logger);
            }),
            development,
            microservice);
#endif

        // await stacksForApplications.SaveAllQueued();
        // await stacksForMicroservices.SaveAllQueued();
        // try
        // {
        //     var appManagerApi = new AppManagerApi(config, "https://ingress832b7458.livelyglacier-aba45b93.norwayeast.azurecontainerapps.io", _jsonSerializerOptions);
        //     await appManagerApi.Authenticate();
        //
        //     await appManagerApi.AddAzureSubscription(config.Azure.Subscription.SubscriptionId.ToString(), config.Azure.Subscription.Name, config.Azure.Subscription.TenantId, config.Azure.Subscription.TenantName);
        //     await appManagerApi.SetAzureServicePrincipal(config.Azure.ServicePrincipal.ClientId, config.Azure.ServicePrincipal.ClientSecret);
        //     await appManagerApi.SetPulumiSettings(config.Pulumi.Organization, config.Pulumi.AccessToken);
        //     await appManagerApi.SetMongoDBSettings(config.MongoDB.OrganizationId, config.MongoDB.PublicKey, config.MongoDB.PrivateKey);
        //
        //     await appManagerApi.CreateApplication(application.Id, application.Name, application.AzureSubscriptionId, application.CloudLocation);
        //     await appManagerApi.CreateEnvironment(application.Id, development);
        //
        //     await appManagerApi.CreateMicroservice(application.Id, development, microservice.Id, microservice.Name);
        //     await appManagerApi.CreateDeployable(application.Id, development, microservice.Id, microservice.Deployables.Single().Id, microservice.Deployables.Single().Name);
        //     await appManagerApi.SetDeployableImage(application.Id, development, microservice.Id, microservice.Deployables.Single().Id, microservice.Deployables.Single().Image);
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine($"Errors with calling API - {ex.Message}");
        // }
        Console.WriteLine("Waiting...");
        Console.ReadLine();
    }

    static async Task<ApplicationAndEnvironment> ApplyConfigAndVariables(ApplicationAndEnvironment applicationAndEnvironment, ManagementConfig config)
    {
        var dockerHub = new DockerHub();
        var cratisVersion = await dockerHub.GetLastVersionOfCratis();
        var appManagerVersion = await dockerHub.GetLastVersionOfAppManager();
        var ingressMiddlewareVersion = await dockerHub.GetLastVersionOfIngressMiddleware();

        var identityProvider = applicationAndEnvironment.Environment.Ingresses.First().IdentityProviders.First();
        return applicationAndEnvironment with
        {
            Environment = applicationAndEnvironment.Environment with
            {
                CratisVersion = cratisVersion,
                AzureSubscriptionId = config.Azure.Subscription.SubscriptionId,
                Resources = new(
                    null!,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!,
                    new(null!, new[] { new MongoDBUser("kernel", config.MongoDB.KernelUserPassword) })),

                Certificates = config.Certificates.Select(
                    c => new Certificate(
                        c.Id,
                        c.Name,
                        Convert.ToBase64String(File.ReadAllBytes(c.File)),
                        c.Password)),
                Ingresses = new[]
                {
                    applicationAndEnvironment.Environment.Ingresses.First() with
                    {
                        MiddlewareVersion = ingressMiddlewareVersion,
                        IdentityProviders = new[]
                        {
                            identityProvider with
                            {
                                ClientId = config.Authentication.ClientId,
                                ClientSecret = config.Authentication.ClientSecret
                            }
                        }
                    }
                },
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
}
