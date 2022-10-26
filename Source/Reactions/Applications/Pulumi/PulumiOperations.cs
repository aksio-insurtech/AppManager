// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using Common;
using Concepts.Applications.Environments;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Pulumi;
using Pulumi.Automation;

namespace Reactions.Applications.Pulumi;

/// <summary>
/// Represents an implementation of <see cref="IPulumiOperations"/>.
/// </summary>
public class PulumiOperations : IPulumiOperations
{
    readonly ILogger<PulumiOperations> _logger;
    readonly ISettings _settings;
    readonly IStacksForApplications _stacksForApplications;
    readonly IStacksForMicroservices _stacksForMicroservices;

    public PulumiOperations(
        ILogger<PulumiOperations> logger,
        ISettings applicationSettings,
        IStacksForApplications stacksForApplications,
        IStacksForMicroservices stacksForMicroservices)
    {
        _logger = logger;
        _settings = applicationSettings;
        _stacksForApplications = stacksForApplications;
        _stacksForMicroservices = stacksForMicroservices;
    }

    /// <inheritdoc/>
    public async Task Up(Application application, string projectName, PulumiFn definition, ApplicationEnvironmentWithArtifacts environment, Microservice? microservice = default)
    {
        _logger.UppingStack();

        try
        {
            _logger.CreatingStack(application.Name ?? "[N/A]");
            var stack = await CreateStack(application, projectName, environment, definition, microservice);

            _logger.RefreshingStack();
            await stack.RefreshAsync();

            _logger.PuttingUpStack();
            await stack.UpAsync(new UpOptions
            {
                OnStandardOutput = Console.WriteLine,
                OnStandardError = Console.Error.WriteLine
            });

            await (microservice is not null ?
                SaveStackForMicroservice(application, microservice, environment, stack) :
                SaveStackForApplication(application, environment, stack));
        }
        catch (Exception ex)
        {
            _logger.Errored(ex);
        }
    }

    /// <inheritdoc/>
    public async Task Down(Application application, string projectName, PulumiFn definition, ApplicationEnvironmentWithArtifacts environment, Microservice? microservice = default)
    {
        try
        {
            _logger.CreatingStack(application.Name ?? "[N/A]");
            var stack = await CreateStack(application, projectName, environment, definition, microservice);

            _logger.RefreshingStack();
            await stack.RefreshAsync();

            _logger.TakingDownStack();
            await stack.DestroyAsync(new DestroyOptions { OnStandardOutput = Console.WriteLine });

            await (microservice is not null ?
                SaveStackForMicroservice(application, microservice, environment, stack) :
                SaveStackForApplication(application, environment, stack));
        }
        catch (Exception ex)
        {
            _logger.Errored(ex);
        }
    }

    /// <inheritdoc/>
    public async Task Remove(Application application, string projectName, PulumiFn definition, ApplicationEnvironmentWithArtifacts environment, Microservice? microservice = default)
    {
        _logger.StackBeingRemoved();

        try
        {
            _logger.CreatingStack(application.Name ?? "[N/A]");
            var stack = await CreateStack(application, projectName, environment, definition, microservice);

            _logger.RefreshingStack();
            await stack.RefreshAsync();

            _logger.RemovingStack();
            await stack.Workspace.RemoveStackAsync(environment.DisplayName);

            await (microservice is not null ?
                SaveStackForMicroservice(application, microservice, environment, stack) :
                SaveStackForApplication(application, environment, stack));
        }
        catch (Exception ex)
        {
            _logger.Errored(ex);
        }
    }

    /// <inheritdoc/>
    public async Task SetTag(string projectName, ApplicationEnvironmentWithArtifacts environment, string tagName, string value)
    {
        var stackName = environment.DisplayName;
        var payload = new
        {
            name = tagName,
            value
        };

        var url = $"https://api.pulumi.com/api/stacks/{_settings.PulumiOrganization}/{projectName}/{stackName}/tags";
        var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.pulumi+8"));
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", _settings.PulumiAccessToken);
        await client.PostAsJsonAsync(url, payload);
    }

    async Task<WorkspaceStack> CreateStack(Application application, string projectName, ApplicationEnvironmentWithArtifacts environment, PulumiFn program, Microservice? microservice = default)
    {
        var stackName = environment.DisplayName;

        var accessToken = _settings.PulumiAccessToken;
        _logger.PulumiInformation($"{accessToken.Value.Substring(0, 4)}*****");

        var mongoDBPublicKey = _settings.MongoDBPublicKey;
        var mongoDBPrivateKey = _settings.MongoDBPrivateKey;

        var args = new InlineProgramArgs(projectName, stackName, program)
        {
            ProjectSettings = new(projectName, ProjectRuntimeName.Dotnet),
            EnvironmentVariables = new Dictionary<string, string?>
            {
                { "TF_LOG", "TRACE" },
                { "PULUMI_ACCESS_TOKEN", accessToken.ToString() },
                { "MONGODB_ATLAS_PUBLIC_KEY", mongoDBPublicKey },
                { "MONGODB_ATLAS_PRIVATE_KEY", mongoDBPrivateKey }
            }
        };

        _logger.CreatingOrSelectingStack();
        var stack = await LocalWorkspace.CreateOrSelectStackAsync(args);

        var hasStack = await (microservice is not null ?
            _stacksForMicroservices.HasFor(application.Id, microservice.Id, environment) :
            _stacksForApplications.HasFor(application.Id, environment));

        if (hasStack)
        {
            _logger.GettingStackDeployment(application.Name);

            var deployment = await (microservice is not null ?
                _stacksForMicroservices.GetFor(application.Id, microservice.Id, environment) :
                _stacksForApplications.GetFor(application.Id, environment));
            await stack.ImportStackAsync(deployment);
        }

        // TODO: This should probably be hidden behind a user action with a big "Are you sure? This could leave things in an inconsistent state".
        var info = await stack.GetInfoAsync();
        if (info?.Result == UpdateState.InProgress)
        {
            await stack.CancelAsync();
        }

        _logger.RemovingPendingOperations();
        await RemovePendingOperations(stack);

        _logger.InstallingPlugins();
        await stack.Workspace.InstallPluginAsync("azure-native", "1.67.0");
        await stack.Workspace.InstallPluginAsync("azuread", "5.26.1");
        await stack.Workspace.InstallPluginAsync("mongodbatlas", "3.5.0");

        _logger.SettingAllConfig();
        await stack.SetAllConfigAsync(new Dictionary<string, ConfigValue>
            {
                { "azure-native:location", new ConfigValue(environment.CloudLocation) },
                { "azure-native:subscriptionId", new ConfigValue(environment.AzureSubscriptionId.ToString()) },
                { "azure-native:clientId", new ConfigValue(_settings.ServicePrincipal.ClientId) },
                { "azure-native:clientSecret", new ConfigValue(_settings.ServicePrincipal.ClientSecret, true) },
                { "azure-native:tenantId", new ConfigValue(_settings.AzureSubscriptions.First().TenantId) }
            });

        await SetTag(projectName, environment, "application", application.Name);
        await SetTag(projectName, environment, "environment", stackName);

        return stack;
    }

    async Task RemovePendingOperations(WorkspaceStack stack)
    {
        _logger.ExportStack();

        var stackDeployment = await stack.ExportStackAsync();
        var jsonNode = JsonNode.Parse(stackDeployment.Json.GetRawText())!.AsObject();
        var deployment = jsonNode!["deployment"]!.AsObject();
        if (deployment["pending_operations"] is not JsonArray pendingOperations || pendingOperations.Count == 0)
        {
            _logger.NoPendingOperations();
            return;
        }

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

    async Task SaveStackForApplication(Application application, ApplicationEnvironment environment, WorkspaceStack stack)
    {
        _logger.SavingStackDeploymentForApplication(application.Name);

        var deployment = await stack.ExportStackAsync();
        await _stacksForApplications.Save(application.Id, environment, deployment);
    }

    async Task SaveStackForMicroservice(Application application, Microservice microservice, ApplicationEnvironment environment, WorkspaceStack stack)
    {
        _logger.SavingStackDeploymentForMicroservice(microservice.Name);

        var deployment = await stack.ExportStackAsync();
        await _stacksForMicroservices.Save(application.Id, microservice.Id, environment, deployment);
    }
}
