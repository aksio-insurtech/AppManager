// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.Cratis.Execution;
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
    readonly IExecutionContextManager _executionContextManager;
    readonly IPulumiStackDefinitions _stackDefinitions;
    readonly IStacksForApplications _stacksForApplications;
    readonly IStacksForMicroservices _stacksForMicroservices;

    public PulumiOperations(
        ISettings applicationSettings,
        IExecutionContextManager executionContextManager,
        IPulumiStackDefinitions stackDefinitions,
        IStacksForApplications stacksForApplications,
        IStacksForMicroservices stacksForMicroservices,
        ILogger<PulumiOperations> logger)
    {
        _logger = logger;
        _settings = applicationSettings;
        _executionContextManager = executionContextManager;
        _stackDefinitions = stackDefinitions;
        _stacksForApplications = stacksForApplications;
        _stacksForMicroservices = stacksForMicroservices;
    }

    /// <summary>
    /// Get the Pulumi project name for a microservice on an application.
    /// </summary>
    /// <param name="application">Application to get for.</param>
    /// <param name="microservice">Microservice to get for.</param>
    /// <returns>Correct Pulumi project name.</returns>
    public static string GetProjectNameFor(Application application, Microservice? microservice = null)
    {
        if (microservice is not null)
        {
            return $"{application.Name}-{microservice.Name}";
        }

        return application.Name.Value;
    }

    /// <inheritdoc/>
    public async Task Up(Application application, PulumiFn definition, ApplicationEnvironmentWithArtifacts environment, Microservice? microservice = default)
    {
        _logger.UppingStack();

        try
        {
            var stack = await CreateStack(application, environment, definition, microservice);
            await RefreshStack(stack);

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
    public async Task Down(Application application, PulumiFn definition, ApplicationEnvironmentWithArtifacts environment, Microservice? microservice = default)
    {
        try
        {
            var stack = await CreateStack(application, environment, definition, microservice);
            await RefreshStack(stack);

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
    public async Task Remove(Application application, PulumiFn definition, ApplicationEnvironmentWithArtifacts environment, Microservice? microservice = default)
    {
        _logger.StackBeingRemoved();

        try
        {
            var stack = await CreateStack(application, environment, definition, microservice);
            await RefreshStack(stack);

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
    public async Task SetTag(Application application, ApplicationEnvironmentWithArtifacts environment, string tagName, string value)
    {
        var stackName = environment.DisplayName;
        var payload = new
        {
            name = tagName,
            value
        };
        var projectName = GetProjectNameFor(application);
        var url = $"https://api.pulumi.com/api/stacks/{_settings.PulumiOrganization}/{projectName}/{stackName}/tags";
        var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.pulumi+8"));
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", _settings.PulumiAccessToken);
        await client.PostAsJsonAsync(url, payload);
    }

    /// <inheritdoc/>
    public async Task ConsolidateEnvironment(Application application, ApplicationEnvironmentWithArtifacts environment)
    {
        ApplicationEnvironmentResult? applicationEnvironmentResult = default;
        var executionContext = _executionContextManager.Current;

        await Up(
            application,
            PulumiFn.Create(async () =>
            {
                applicationEnvironmentResult = await _stackDefinitions.ApplicationEnvironment(executionContext, application, environment, environment.CratisVersion);
                environment = await applicationEnvironmentResult.MergeWithApplicationEnvironment(environment);

                foreach (var ingress in environment.Ingresses)
                {
                    await _stackDefinitions.Ingress(executionContext, application, environment, ingress, applicationEnvironmentResult.ResourceGroup);
                }
            }),
            environment);

        if (environment.Resources?.AzureResourceGroupId is null)
        {
            return;
        }

        foreach (var microservice in environment.Microservices)
        {
            await Up(
                application,
                PulumiFn.Create(async () =>
                {
                    var resourceGroup = application.GetResourceGroup(environment);
                    var storage = await application.GetStorage(environment, applicationEnvironmentResult!.ResourceGroup);
                    var microserviceResult = await _stackDefinitions.Microservice(
                        executionContext,
                        application,
                        microservice,
                        environment,
                        false,
                        resourceGroup: resourceGroup,
                        deployables: microservice.Deployables);
                }),
                environment,
                microservice);
        }
    }

    async Task<WorkspaceStack> CreateStack(Application application, ApplicationEnvironmentWithArtifacts environment, PulumiFn program, Microservice? microservice = default)
    {
        _logger.CreatingStack(application.Name);
        var stackName = environment.DisplayName;

        var accessToken = _settings.PulumiAccessToken;
        _logger.PulumiInformation($"{accessToken.Value.Substring(0, 4)}*****");

        var mongoDBPublicKey = _settings.MongoDBPublicKey;
        var mongoDBPrivateKey = _settings.MongoDBPrivateKey;

        var projectName = GetProjectNameFor(application, microservice);

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
        await stack.Workspace.InstallPluginAsync("azure-native", "1.83.1");
        await stack.Workspace.InstallPluginAsync("azuread", "5.26.1");
        await stack.Workspace.InstallPluginAsync("mongodbatlas", "3.5.2");

        _logger.SettingAllConfig();
        await stack.SetAllConfigAsync(new Dictionary<string, ConfigValue>
            {
                { "azure-native:location", new ConfigValue(environment.CloudLocation) },
                { "azure-native:subscriptionId", new ConfigValue(environment.AzureSubscriptionId.ToString()) },
                { "azure-native:clientId", new ConfigValue(_settings.ServicePrincipal.ClientId) },
                { "azure-native:clientSecret", new ConfigValue(_settings.ServicePrincipal.ClientSecret, true) },
                { "azure-native:tenantId", new ConfigValue(_settings.AzureSubscriptions.First().TenantId) }
            });

        await SetTag(application, environment, "application", application.Name);
        await SetTag(application, environment, "environment", stackName);

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

    async Task RefreshStack(WorkspaceStack stack)
    {
        _logger.RefreshingStack();
        await stack.RefreshAsync(new()
        {
            OnStandardOutput = Console.WriteLine,
            OnStandardError = Console.Error.WriteLine
        });
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
