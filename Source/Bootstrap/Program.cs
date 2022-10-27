// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Aksio.Cratis.Execution;
using Aksio.Cratis.Json;
using Concepts.Azure;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Pulumi.Automation;
using Reactions.Applications;
using Reactions.Applications.Pulumi;
using Read.Settings;
using MicroserviceId = Concepts.Applications.MicroserviceId;

namespace Bootstrap;

public static class Program
{
    static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
                {
                    new ConceptAsJsonConverterFactory()
                }
    };

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

        var config = JsonSerializer.Deserialize<ManagementConfig>(configAsJson, _jsonSerializerOptions)!;

        var dockerHub = new DockerHub();
        var cratisVersion = await dockerHub.GetLastVersionOfCratis();

        var settings = new Settings(
            new AzureSubscription[] { config.Azure.Subscription },
            config.Pulumi.Organization,
            config.Pulumi.AccessToken,
            config.MongoDB.OrganizationId,
            config.MongoDB.PublicKey,
            config.MongoDB.PrivateKey,
            config.Azure.ServicePrincipal);

        var aksioTenant = (TenantId)"d92e197b-7dcb-4893-a762-df334665f0d2";
        var gammaId = (MicroserviceId)"8c538618-2862-4018-b29d-17a4ec131958";

        var development = new ApplicationEnvironmentWithArtifacts(
            Guid.Parse("00126dcd-8d1e-42c3-835b-7978a545ec5c"),
            "Development",
            "dev",
            "D",
            cratisVersion,
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
            new[] { new Tenant(aksioTenant, "Aksio Insurtech") },
            Enumerable.Empty<Ingress>(),
            Enumerable.Empty<Microservice>());

        var application = new Application(
            Guid.Parse("1091c7d3-f533-420d-abc0-bbb7f0defd66"),
            "AppManager",
            new[] { development });

        var executionContextManager = new ExecutionContextManager();
        var eventLog = new InMemoryEventLog();
        var executionContext = new ExecutionContext(Aksio.Cratis.Execution.MicroserviceId.Unspecified, TenantId.Development, CorrelationId.New(), CausationId.System, CausedBy.System);

        var logger = loggerFactory.CreateLogger<FileStorage>();
        var definitions = new PulumiStackDefinitions(settings, executionContextManager, eventLog, logger);

        var stacksForApplications = new BootstrapStacksForApplications();
        var stacksForMicroservices = new BootstrapStacksForMicroservices(application.Id);
        var operations = new PulumiOperations(
            loggerFactory.CreateLogger<PulumiOperations>(),
            settings,
            stacksForApplications,
            stacksForMicroservices);

        ApplicationEnvironmentResult? applicationEnvironmentResult = default;
        var ingress = new Ingress(
            Guid.Parse("6173e1f6-edee-423e-943a-e4bbc90349ce"),
            "main",
            new[] { new Route("/", gammaId, "/") });
        IngressResult? ingressResult = default;

        await operations.Up(
            application,
            config.Name,
            PulumiFn.Create(async () =>
            {
                applicationEnvironmentResult = await definitions.ApplicationEnvironment(executionContext, application, development, cratisVersion);
                application = await applicationEnvironmentResult.MergeWithApplication(application, development);
                ingressResult = await definitions.Ingress(executionContext, application, development, ingress, applicationEnvironmentResult.ResourceGroup);

                Console.WriteLine("\n\nSetup AppManager as application");
                var appManagerApi = new AppManagerApi(config, ingressResult.Url, _jsonSerializerOptions);
                await appManagerApi.Authenticate();

                stacksForApplications.AppManagerApi = appManagerApi;
                stacksForMicroservices.AppManagerApi = appManagerApi;
            }),
            development);

        var appManagerVersion = await dockerHub.GetLastVersionOfAppManager();
        var microservice = new Microservice(
            gammaId,
            application.Id,
            "Gamma",
            new Deployable[]
            {
                new Deployable(
                    Guid.Parse("439b3c29-759b-4a03-92a7-d36a59be9ade"),
                    gammaId,
                    "main",
                    $"docker.io/aksioinsurtech/app-manager:{appManagerVersion}",
                    new[] { 80 })
            });

        await operations.Up(
            application,
            $"{config.Name}-{microservice.Name}",
            PulumiFn.Create(async () =>
            {
                if (applicationEnvironmentResult is null)
                {
                    return;
                }

                var storage = await application.GetStorage(development, applicationEnvironmentResult!.ResourceGroup);

                application = await applicationEnvironmentResult.MergeWithApplication(application, development);
                var microserviceResult = await definitions.Microservice(
                    executionContext,
                    application,
                    microservice,
                    development,
                    false,
                    resourceGroup: applicationEnvironmentResult!.ResourceGroup,
                    deployables: microservice.Deployables);

                var fileShare = Pulumi.AzureNative.Storage.FileShare.Get(ingressResult!.FileShareName, ingressResult!.FileShareId);
                var microserviceResourceName = await microserviceResult.ContainerApp.Name.GetValue();
                await application.ConfigureIngress(
                    applicationEnvironmentResult.ResourceGroup,
                    ingress,
                    storage,
                    fileShare,
                    new[] { microservice },
                    logger);
            }),
            development,
            microservice);

        await stacksForApplications.SaveAllQueued();
        await stacksForMicroservices.SaveAllQueued();

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
}
