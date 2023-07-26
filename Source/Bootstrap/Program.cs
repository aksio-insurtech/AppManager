// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Aksio.Execution;
using Aksio.Json;
using Aksio.Serialization;
using Aksio.Types;
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

#pragma warning disable CA2000 // Dispose objects before losing scope - LoggerFactory will stay with us
        var loggerFactory = LoggerFactory.Create(_ => _.AddConsole());
#pragma warning restore CA2000 // Dispose objects before losing scope
        var types = new Types();
        var derivedTypes = new DerivedTypes(types);
        Globals.Configure(derivedTypes);

        var serializerOptions = Globals.JsonSerializerOptions;
        serializerOptions.PropertyNameCaseInsensitive = true;
        serializerOptions.Converters.Add(new SemanticVersionJsonConverter());

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

        var executionContextManager = new ExecutionContextManager();
        var eventLog = new InMemoryEventLog();
        var executionContext = new ExecutionContext(MicroserviceId.Unspecified, TenantId.Development, CorrelationId.New(), CausationId.System, CausedBy.System);
        executionContextManager.Set(executionContext);

        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterInstance(executionContextManager).As<IExecutionContextManager>();
        containerBuilder.RegisterInstance(eventLog).As<IEventLog>();
        containerBuilder.RegisterInstance(loggerFactory).As<ILoggerFactory>();
        containerBuilder.RegisterDefaults(types);
        var serviceProvider = new AutofacServiceProviderFactory().CreateServiceProvider(containerBuilder);

        var applicationAndEnvironmentAsJson = await File.ReadAllTextAsync(filename);
        var applicationAndEnvironment = JsonSerializer.Deserialize<ApplicationAndEnvironment>(applicationAndEnvironmentAsJson, serializerOptions)!;
        applicationAndEnvironment = await applicationAndEnvironment.ApplyConfigAndVariables(Path.GetDirectoryName(filename)!, config);

        // Do some validation, this throws exceptions if a problem is detected.
        applicationAndEnvironment.Environment.ValidateConfiguration();

        var application = new Application(applicationAndEnvironment.Id, applicationAndEnvironment.Name, new(config.Azure.SharedSubscriptionId));

        var logger = loggerFactory.CreateLogger<FileStorage>();
        var definitions = new PulumiStackDefinitions(settings, executionContextManager, logger);

        var resourceRenderers = new ResourceRendering(types, serviceProvider);
        var stacksForApplications = new BootstrapStacksForApplications();
        var stacksForMicroservices = new BootstrapStacksForMicroservices(application.Id);
        var operations = new PulumiOperations(
            settings,
            executionContextManager,
            definitions,
            stacksForApplications,
            stacksForMicroservices,
            resourceRenderers,
            new ApplicationEnvironmentDeploymentLog(),
            loggerFactory.CreateLogger<PulumiOperations>(),
            loggerFactory.CreateLogger<FileStorage>());

        await operations.ConsolidateEnvironment(application, applicationAndEnvironment.Environment, ApplicationEnvironmentDeploymentId.New());

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
