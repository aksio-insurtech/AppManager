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
        var (variableFile, environmentFile) = DetermineConfigFiles(args);
        if (string.IsNullOrEmpty(variableFile))
        {
            return;
        }

        // Help user to not accidentally deploy the wrong environment.
        Console.WriteLine($"Applying environment {environmentFile} with variables from {variableFile}.");
        Console.WriteLine("Press enter to continue");
        Console.ReadLine();

#pragma warning disable CA2000// Dispose objects before losing scope - LoggerFactory will stay with us
        var loggerFactory = LoggerFactory.Create(_ => _.AddConsole());
#pragma warning restore CA2000// Dispose objects before losing scope
        var types = new Types();
        var derivedTypes = new DerivedTypes(types);
        Globals.Configure(derivedTypes);

        var serializerOptions = Globals.JsonSerializerOptions;
        serializerOptions.PropertyNameCaseInsensitive = true;
        serializerOptions.Converters.Add(new SemanticVersionJsonConverter());

        var configAsJson = await File.ReadAllTextAsync(variableFile);
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
        var executionContext = new ExecutionContext(
            MicroserviceId.Unspecified,
            TenantId.Development,
            CorrelationId.New(),
            CausationId.System,
            CausedBy.System);
        executionContextManager.Set(executionContext);

        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterInstance(executionContextManager).As<IExecutionContextManager>();
        containerBuilder.RegisterInstance(eventLog).As<IEventLog>();
        containerBuilder.RegisterInstance(loggerFactory).As<ILoggerFactory>();
        containerBuilder.RegisterDefaults(types);
        var serviceProvider = new AutofacServiceProviderFactory().CreateServiceProvider(containerBuilder);

        var applicationAndEnvironmentAsJson = await File.ReadAllTextAsync(environmentFile);

        var applicationAndEnvironment =
            JsonSerializer.Deserialize<ApplicationAndEnvironment>(applicationAndEnvironmentAsJson, serializerOptions)!;
        applicationAndEnvironment =
            await applicationAndEnvironment.ApplyConfigAndVariables(Path.GetDirectoryName(environmentFile)!, config);

        // Do some validation, this throws exceptions if a problem is detected.
        applicationAndEnvironment.Environment.ValidateConfiguration();

        var application = new Application(
            applicationAndEnvironment.Id,
            applicationAndEnvironment.Name,
            new(config.Azure.SharedSubscriptionId));

        var fileStorageLogger = loggerFactory.CreateLogger<FileStorage>();
        var definitions = new PulumiStackDefinitions(
            settings,
            executionContextManager,
            fileStorageLogger);

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

        /*
        // Helper to debug ingress config builders
        foreach (var ingress in applicationAndEnvironment.Environment.Ingresses)
        {
            var ingressConfig = await ingress.RenderMiddlewareTemplate(
                applicationAndEnvironment.Environment,
                new Dictionary<Concepts.Applications.MicroserviceId, ContainerApp>());
        }
        */

        await operations.ConsolidateEnvironment(
            application,
            applicationAndEnvironment.Environment,
            ApplicationEnvironmentDeploymentId.New());

        Console.WriteLine("Done. Hit any key.");
        Console.ReadKey();
    }

    /// <summary>
    /// Figure out which input files to use.
    /// Will terminate application if none are available.
    /// </summary>
    /// <param name="args">Commandline args.</param>
    static (string VariableFile, string EnvironmentFile) DetermineConfigFiles(string[] args)
    {
        if (args.Length == 1)
        {
            return (args[0], "./AppManager.json");
        }

        if (args.Length == 2)
        {
            return (args[0], args[1]);
        }

        Console.WriteLine($"Usage: {AppDomain.CurrentDomain.FriendlyName} variablefile.json [configfile.json]");
        Console.WriteLine("This app must be called with zero, one or two arguments.");
        return (string.Empty, string.Empty);
    }
}