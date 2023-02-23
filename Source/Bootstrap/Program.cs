// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Aksio.Cratis.Execution;
using Aksio.Cratis.Json;
using Aksio.Cratis.Serialization;
using Aksio.Cratis.Types;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Concepts;
using Concepts.Applications.Environments;
using Microsoft.Extensions.Logging;
using Reactions.Applications;
using Reactions.Applications.Pulumi;
using Reactions.Applications.Pulumi.Resources;
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

        var filename = "./AppManager.json";
        if (args.Length == 2)
        {
            filename = args[1];
        }

        Types.AddAssemblyPrefixesToExclude(
            "AutoMapper",
            "Autofac",
            "Azure",
            "Elasticsearch",
            "FluentValidation",
            "Handlebars",
            "Humanizer",
            "NJsonSchema",
            "MongoDB",
            "Orleans",
            "Serilog",
            "Swashbuckle",
            "Pulumi",
            "Grpc",
            "Namotion",
            "YamlDotNet",
            "OneOf",
            "Azure",
            "Ben",
            "CliWrap",
            "DnsClient",
            "Semver",
            "SharpCompress");

        var types = new Types();
        var derivedTypes = new DerivedTypes(types);
        Globals.Configure(derivedTypes);
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterDefaults(types);
        var serviceProvider = new AutofacServiceProviderFactory().CreateServiceProvider(containerBuilder);

        var loggerFactory = LoggerFactory.Create(_ => _.AddConsole());
        var serializerOptions = Globals.JsonSerializerOptions;
        serializerOptions.Converters.Add(new SemanticVersionJsonConverter());

        // Setup application within cloud environment
        // Wait till its ready and then append the events that represents the actions done (through commands?)
        var configAsJson = await File.ReadAllTextAsync(args[0]);
        var config = JsonSerializer.Deserialize<ManagementConfig>(configAsJson, serializerOptions)!;

        var settings = new Settings(
            config.Azure.Subscriptions,
            config.Pulumi.Organization,
            config.Pulumi.AccessToken,
            config.MongoDB.OrganizationId,
            config.MongoDB.PublicKey,
            config.MongoDB.PrivateKey,
            config.Azure.ServicePrincipal);

        var applicationAndEnvironmentAsJson = await File.ReadAllTextAsync(filename);
        var applicationAndEnvironment = JsonSerializer.Deserialize<ApplicationAndEnvironment>(applicationAndEnvironmentAsJson, serializerOptions)!;
        applicationAndEnvironment = await applicationAndEnvironment.ApplyConfigAndVariables(config);
        var application = new Application(applicationAndEnvironment.Id, applicationAndEnvironment.Name, new(config.Azure.SharedSubscriptionId));

        var executionContextManager = new ExecutionContextManager();
        var eventLog = new InMemoryEventLog();
        var executionContext = new ExecutionContext(MicroserviceId.Unspecified, TenantId.Development, CorrelationId.New(), CausationId.System, CausedBy.System);
        executionContextManager.Set(executionContext);

        var logger = loggerFactory.CreateLogger<FileStorage>();
        var definitions = new PulumiStackDefinitions(settings, executionContextManager, eventLog, logger);

        var resourceRenderers = new ResourceRenderers(types, serviceProvider);
        var stacksForApplications = new BootstrapStacksForApplications();
        var stacksForMicroservices = new BootstrapStacksForMicroservices(application.Id);
        var operations = new PulumiOperations(
            settings,
            executionContextManager,
            definitions,
            stacksForApplications,
            stacksForMicroservices,
            resourceRenderers,
            new ApplicationEnvironmentConsolidationLog(),
            loggerFactory.CreateLogger<PulumiOperations>(),
            loggerFactory.CreateLogger<FileStorage>());

        await operations.ConsolidateEnvironment(application, applicationAndEnvironment.Environment, ApplicationEnvironmentConsolidationId.New());

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
        Console.WriteLine("Done... Hit any key..");
        Console.ReadLine();
    }
}
