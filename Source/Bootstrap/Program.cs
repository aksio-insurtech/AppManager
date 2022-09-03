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
        var operations = new PulumiOperations(loggerFactory.CreateLogger<PulumiOperations>(), settings);

        operations.Up(
            application,
            config.Name,
            PulumiFn.Create(async () =>
            {
                var applicationResult = await definitions.Application(executionContext, application, cloudRuntimeEnvironment, true);

                var microservice = new Microservice(
                    Guid.Parse("8c538618-2862-4018-b29d-17a4ec131958"),
                    application.Id,
                    "AppManager");

                var appManagerVersion = await DockerHub.GetLatestVersionOfImage("aksioinsurtech", "app-manager");

                application = await applicationResult.MergeWithApplication(application);

                var deployable = new Deployable(
                            Guid.Parse("439b3c29-759b-4a03-92a7-d36a59be9ade"),
                            microservice.Id,
                            "main",
                            $"docker.io/aksioinsurtech/app-manager:{appManagerVersion}",
                            new[] { 80 });

                var microserviceResult = await definitions.Microservice(
                    executionContext,
                    application,
                    microservice,
                    cloudRuntimeEnvironment,
                    false,
                    applicationResult.ResourceGroup,
                    new[]
                    {
                        deployable
                    });

                var microserviceResourceName = await microserviceResult.ContainerApp.Name.GetValue();

                var ingressUrl = await application.SetupIngress(
                    applicationResult.ResourceGroup,
                    applicationResult.Storage,
                    applicationResult.ManagedEnvironment,
                    application.GetTags(cloudRuntimeEnvironment),
                    logger,
                    microserviceResourceName);

                try
                {
                    Console.WriteLine("\n\nSetup AppManager as application");
                    var appManagerApi = new AppManagerApi(config, ingressUrl);
                    await appManagerApi.Authenticate();

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
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Errors with calling API - {ex.Message}");
                }
            }),
            cloudRuntimeEnvironment);

        // new MongoDBEventSequenceStorageProvider()
        Console.WriteLine("Waiting...");
        Console.ReadLine();
    }
}
