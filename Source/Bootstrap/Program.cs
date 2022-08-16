// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Aksio.Cratis.Execution;
using Aksio.Cratis.Json;
using Concepts.Azure;
using Microsoft.Extensions.Logging;
using Reactions.Applications;
using Reactions.Applications.Pulumi;
using Read.Organizations;

namespace Bootstrap;

public static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Missing config file as parameter");
            return;
        }

        var loggerFactory = LoggerFactory.Create(_ => _.AddConsole());

        // Setup application within cloud environment
        // Wait till its ready and then append the events that represents the actions done (through commands?)
        var configAsJson = File.ReadAllText(args[0]);

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
            config.MongoDB.Organization,
            config.MongoDB.PublicKey,
            config.MongoDB.PrivateKey,
            config.Azure.ServicePrincipal,
            config.Elasticsearch.Url,
            config.Elasticsearch.ApiKey);

        var application = new Application(
            Guid.NewGuid(),
            "Aksio Cloud Management",
            config.Azure.Subscription.SubscriptionId,
            config.CloudLocation,
            null!,
            null!);

        var executionContextManager = new ExecutionContextManager();
        var eventLog = new InMemoryEventLog();
        var executionContext = new ExecutionContext(MicroserviceId.Unspecified, TenantId.Development, CorrelationId.New(), CausationId.System, CausedBy.System);

        var definitions = new PulumiStackDefinitions(settings, executionContextManager, eventLog, loggerFactory.CreateLogger<FileStorage>());
        var definition = definitions.Application(executionContext, application, Concepts.CloudRuntimeEnvironment.Development);

        var operations = new PulumiOperations(loggerFactory.CreateLogger<PulumiOperations>(), settings);
        operations.Up(application, config.Name, definition, Concepts.CloudRuntimeEnvironment.Development);

        Console.WriteLine("Waiting...");
        Console.ReadLine();
    }
}
