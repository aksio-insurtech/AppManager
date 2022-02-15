// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Common;
using Pulumi.Automation;

namespace Reactions.Applications
{
    /// <summary>
    /// Represents an implementation of <see cref="IPulumiRunner"/>.
    /// </summary>
    public class PulumiRunner : IPulumiRunner
    {
        readonly IApplicationSettings _applicationSettings;

        public PulumiRunner(IApplicationSettings applicationSettings) => _applicationSettings = applicationSettings;

        /// <inheritdoc/>
        public void Up(Application application, string name, PulumiFn definition, RuntimeEnvironment environment)
        {
            _ = Task.Run(async () =>
            {
                var stack = await CreateStack(application, name, environment, definition);
                await stack.UpAsync(new UpOptions { OnStandardOutput = Console.WriteLine });
            });
        }

        /// <inheritdoc/>
        public void Down(Application application, string name, PulumiFn definition, RuntimeEnvironment environment)
        {
            _ = Task.Run(async () =>
            {
                var stack = await CreateStack(application, name, environment, definition);
                await stack.DestroyAsync(new DestroyOptions { OnStandardOutput = Console.WriteLine });
            });
        }

        /// <inheritdoc/>
        public void Remove(Application application, string name, PulumiFn definition, RuntimeEnvironment environment)
        {
            _ = Task.Run(async () =>
            {
                var stack = await CreateStack(application, name, environment, definition);
                await stack.Workspace.RemoveStackAsync("dev");
            });
        }

        async Task<WorkspaceStack> CreateStack(Application application, string name, RuntimeEnvironment environment, PulumiFn program)
        {
            var args = new InlineProgramArgs(name, "dev", program)
            {
                ProjectSettings = new(name, ProjectRuntimeName.Dotnet)
            };
            var stack = await LocalWorkspace.CreateOrSelectStackAsync(args);
            await stack.Workspace.InstallPluginAsync("azure-native", "1.54.0");
            await stack.Workspace.InstallPluginAsync("mongodbatlas", "3.2.0");
            await stack.SetAllConfigAsync(new Dictionary<string, ConfigValue>
            {
                { "azure-native:location", new ConfigValue(application.CloudLocation) },
                { "azure-native:subscriptionId", new ConfigValue(application.AzureSubscriptionId.ToString()) }
            });

            var mongoDBPublicKey = _applicationSettings.MongoDBPublicKey;
            var mongoDBPrivateKey = _applicationSettings.MongoDBPrivateKey;
            Environment.SetEnvironmentVariable("MONGODB_ATLAS_PUBLIC_KEY", mongoDBPublicKey);
            Environment.SetEnvironmentVariable("MONGODB_ATLAS_PRIVATE_KEY", mongoDBPrivateKey);

            var accessToken = _applicationSettings.PulumiAccessToken;
            Environment.SetEnvironmentVariable("PULUMI_ACCESS_TOKEN", accessToken.ToString());

            return stack;
        }
    }
}
