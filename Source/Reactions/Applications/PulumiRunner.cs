// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using Common;
using Concepts;
using Microsoft.Extensions.Logging;
using Pulumi.Automation;

namespace Reactions.Applications
{
    /// <summary>
    /// Represents an implementation of <see cref="IPulumiRunner"/>.
    /// </summary>
    public class PulumiRunner : IPulumiRunner
    {
        readonly ILogger<PulumiRunner> _logger;
        readonly ISettings _settings;

        public PulumiRunner(ILogger<PulumiRunner> logger, ISettings applicationSettings)
        {
            _logger = logger;
            _settings = applicationSettings;
        }

        /// <inheritdoc/>
        public void Up(Application application, string name, PulumiFn definition, CloudRuntimeEnvironment environment)
        {
            _ = Task.Run(async () =>
            {
                var stack = await CreateStack(application, name, environment, definition);
                await stack.UpAsync(new UpOptions { OnStandardOutput = Console.WriteLine });
            });
        }

        /// <inheritdoc/>
        public void Down(Application application, string name, PulumiFn definition, CloudRuntimeEnvironment environment)
        {
            _ = Task.Run(async () =>
            {
                var stack = await CreateStack(application, name, environment, definition);
                await stack.DestroyAsync(new DestroyOptions { OnStandardOutput = Console.WriteLine });
            });
        }

        /// <inheritdoc/>
        public void Remove(Application application, string name, PulumiFn definition, CloudRuntimeEnvironment environment)
        {
            _ = Task.Run(async () =>
            {
                var stack = await CreateStack(application, name, environment, definition);
                await stack.Workspace.RemoveStackAsync("dev");
            });
        }

        async Task<WorkspaceStack> CreateStack(Application application, string name, CloudRuntimeEnvironment environment, PulumiFn program)
        {
            var args = new InlineProgramArgs(name, "dev", program)
            {
                ProjectSettings = new(name, ProjectRuntimeName.Dotnet)
            };
            var stack = await LocalWorkspace.CreateOrSelectStackAsync(args);
            await RemovePendingOperations(stack);
            await stack.Workspace.InstallPluginAsync("azure-native", "1.54.0");
            await stack.Workspace.InstallPluginAsync("mongodbatlas", "3.2.0");
            await stack.SetAllConfigAsync(new Dictionary<string, ConfigValue>
            {
                { "azure-native:location", new ConfigValue(application.CloudLocation) },
                { "azure-native:subscriptionId", new ConfigValue(application.AzureSubscriptionId.ToString()) }
            });

            var mongoDBPublicKey = _settings.MongoDBPublicKey;
            var mongoDBPrivateKey = _settings.MongoDBPrivateKey;
            Environment.SetEnvironmentVariable("MONGODB_ATLAS_PUBLIC_KEY", mongoDBPublicKey);
            Environment.SetEnvironmentVariable("MONGODB_ATLAS_PRIVATE_KEY", mongoDBPrivateKey);

            var accessToken = _settings.PulumiAccessToken;
            Environment.SetEnvironmentVariable("PULUMI_ACCESS_TOKEN", accessToken.ToString());

            return stack;
        }

        async Task RemovePendingOperations(WorkspaceStack stack)
        {
            _logger.ExportStack();

            var stackDeployment = await stack.ExportStackAsync();
            var jsonNode = JsonNode.Parse(stackDeployment.Json.GetRawText())!.AsObject();
            var deployment = jsonNode!["deployment"]!.AsObject();
            deployment.Remove("pending_operations");
            deployment.Add("pending_operations", new JsonArray());
            var jsonString = jsonNode.ToJsonString(new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            stackDeployment = StackDeployment.FromJsonString(jsonString);

            _logger.ImportStack();
            await stack.ImportStackAsync(stackDeployment);
        }
    }
}
