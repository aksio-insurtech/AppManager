// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Aksio.Cratis.Execution;
using Aksio.Cratis.Json;
using Concepts;
using Concepts.Azure;
using Microsoft.Extensions.Logging;
using Pulumi.Automation;
using Reactions.Applications;
using Reactions.Applications.Pulumi;
using Read.Organizations;

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

        // Setup application within cloud environment
        // Wait till its ready and then append the events that represents the actions done (through commands?)
        var configAsJson = await File.ReadAllTextAsync(args[0]);

        var config = JsonSerializer.Deserialize<ManagementConfig>(configAsJson, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
                {
                    new ConceptAsJsonConverterFactory()
                }
        })!;

        // var appManagerApi = new AppManagerApi(config, "https://ingress832b7458.livelyglacier-aba45b93.norwayeast.azurecontainerapps.io");
        // await appManagerApi.Authenticate();
        // BootstrapStacksForApplications.AppManagerApi = appManagerApi;
        // BootstrapStacksForMicroservices.AppManagerApi = appManagerApi;
        // var stack = await File.ReadAllTextAsync("/Users/einari/Projects/Aksio/stack.json");
        // var stackAsDynamic = JsonSerializer.Deserialize<dynamic>(stack);
        // await appManagerApi.SetStack(Guid.Parse("1091c7d3-f533-420d-abc0-bbb7f0defd66"), stack);
        // Console.WriteLine("Wholy cow...");
        // Console.ReadLine();
        var settings = new Settings(
            new AzureSubscription[] { config.Azure.Subscription },
            config.Pulumi.Organization,
            config.Pulumi.AccessToken,
            config.MongoDB.OrganizationId,
            config.MongoDB.PublicKey,
            config.MongoDB.PrivateKey,
            config.Azure.ServicePrincipal);

        var application = new Application(
            Guid.Parse("1091c7d3-f533-420d-abc0-bbb7f0defd66"),
            "AppManager",
            config.Azure.Subscription.SubscriptionId,
            config.CloudLocation,
            new(
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                new(null!, new[] { new MongoDBUser("kernel", config.MongoDB.KernelUserPassword) })),
            new(config.Authentication.ClientId, config.Authentication.ClientSecret));

        var executionContextManager = new ExecutionContextManager();
        var eventLog = new InMemoryEventLog();
        var executionContext = new ExecutionContext(MicroserviceId.Unspecified, TenantId.Development, CorrelationId.New(), CausationId.System, CausedBy.System);

        const CloudRuntimeEnvironment cloudRuntimeEnvironment = CloudRuntimeEnvironment.Development;

        var logger = loggerFactory.CreateLogger<FileStorage>();
        var definitions = new PulumiStackDefinitions(settings, executionContextManager, eventLog, logger);

        var stacksForApplications = new BootstrapStacksForApplications();
        var stacksForMicroservices = new BootstrapStacksForMicroservices(application.Id);
        var operations = new PulumiOperations(
            loggerFactory.CreateLogger<PulumiOperations>(),
            settings,
            stacksForApplications,
            stacksForMicroservices);

        ApplicationResult? applicationResult = default;
        ContainerAppResult? microserviceResult = default;

        await operations.Up(
            application,
            config.Name,
            PulumiFn.Create(async () =>
            {
                applicationResult = await definitions.Application(executionContext, application, cloudRuntimeEnvironment);

                Console.WriteLine("\n\nSetup AppManager as application");
                var appManagerApi = new AppManagerApi(config, applicationResult.Ingress.Url);
                await appManagerApi.Authenticate();

                stacksForApplications.AppManagerApi = appManagerApi;
                stacksForMicroservices.AppManagerApi = appManagerApi;
            }),
            cloudRuntimeEnvironment);

        await operations.Up(
            application,
            $"{config.Name}-AppManager",
            PulumiFn.Create(async () =>
            {
                if (applicationResult is null)
                {
                    return;
                }

                application = await applicationResult.MergeWithApplication(application);
                var appManagerVersion = await DockerHub.GetLatestVersionOfImage("aksioinsurtech", "app-manager");

                var microservice = new Microservice(
                    Guid.Parse("8c538618-2862-4018-b29d-17a4ec131958"),
                    application.Id,
                    "AppManager");

                var deployable = new Deployable(
                            Guid.Parse("439b3c29-759b-4a03-92a7-d36a59be9ade"),
                            microservice.Id,
                            "main",
                            $"docker.io/aksioinsurtech/app-manager:{appManagerVersion}",
                            new[] { 80 });

                microserviceResult = await definitions.Microservice(
                    executionContext,
                    application,
                    microservice,
                    cloudRuntimeEnvironment,
                    false,
                    deployables: new[]
                    {
                        deployable
                    });

                var fileShare = Pulumi.AzureNative.Storage.FileShare.Get(ApplicationIngressPulumiExtensions.IngressFileShareName, applicationResult.Ingress.FileShareId);
                var microserviceResourceName = await microserviceResult.ContainerApp.Name.GetValue();
                await application.ConfigureIngress(applicationResult.ResourceGroup, applicationResult.Storage, fileShare, logger, microserviceResourceName);

                await stacksForApplications.SaveAllQueued();
                await stacksForMicroservices.SaveAllQueued();
            }),
            cloudRuntimeEnvironment);

        // try
        // {
        // await appManagerApi.RegisterOrganization(config.TenantId, config.OrganizationName);
        // await appManagerApi.AddAzureSubscription(config.Azure.Subscription.SubscriptionId.ToString(), config.Azure.Subscription.Name, config.Azure.Subscription.TenantId, config.Azure.Subscription.TenantName);
        // await appManagerApi.SetAzureServicePrincipal(config.Azure.ServicePrincipal.ClientId, config.Azure.ServicePrincipal.ClientSecret);
        // await appManagerApi.SetPulumiSettings(config.Pulumi.Organization, config.Pulumi.AccessToken);
        // await appManagerApi.SetMongoDBSettings(config.MongoDB.OrganizationId, config.MongoDB.PublicKey, config.MongoDB.PrivateKey);
        // await appManagerApi.CreateApplication(application.Id, application.Name, application.AzureSubscriptionId, application.CloudLocation);
        // await appManagerApi.ConfigureAuthenticationForApplication(application.Id, config.Authentication.ClientId, config.Authentication.ClientSecret);
        // await appManagerApi.CreateMicroservice(application.Id, microservice.Id, microservice.Name);
        // await appManagerApi.CreateDeployable(application.Id, microservice.Id, deployable.Id, deployable.Name);
        // await appManagerApi.SetDeployableImage(application.Id, microservice.Id, deployable.Id, deployable.Image);
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine($"Errors with calling API - {ex.Message}");
        // }
        // new MongoDBEventSequenceStorageProvider()
        Console.WriteLine("Waiting...");
        Console.ReadLine();
    }
}
