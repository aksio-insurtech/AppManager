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
using Pulumi.AzureNative.App;
using Pulumi.AzureNative.Resources;
using Reactions.Applications.Pulumi.Resources;
using Read.Applications.Environments;
using MicroserviceId = Concepts.Applications.MicroserviceId;

namespace Reactions.Applications.Pulumi;

/// <summary>
/// Represents an implementation of <see cref="IPulumiOperations"/>.
/// </summary>
public class PulumiOperations : IPulumiOperations
{
    readonly ILogger<PulumiOperations> _logger;
    readonly ILogger<FileStorage> _fileStorageLogger;
    readonly ISettings _settings;
    readonly IExecutionContextManager _executionContextManager;
    readonly IPulumiStackDefinitions _stackDefinitions;
    readonly IStacksForApplications _stacksForApplications;
    readonly IStacksForMicroservices _stacksForMicroservices;
    readonly IResourceRenderers _resourceRenderers;
    readonly IApplicationEnvironmentDeploymentLog _applicationEnvironmentDeploymentLog;

    public PulumiOperations(
        ISettings applicationSettings,
        IExecutionContextManager executionContextManager,
        IPulumiStackDefinitions stackDefinitions,
        IStacksForApplications stacksForApplications,
        IStacksForMicroservices stacksForMicroservices,
        IResourceRenderers resourceRenderers,
        IApplicationEnvironmentDeploymentLog applicationEnvironmentDeploymentLog,
        ILogger<PulumiOperations> logger,
        ILogger<FileStorage> fileStorageLogger)
    {
        _logger = logger;
        _fileStorageLogger = fileStorageLogger;
        _settings = applicationSettings;
        _executionContextManager = executionContextManager;
        _stackDefinitions = stackDefinitions;
        _stacksForApplications = stacksForApplications;
        _stacksForMicroservices = stacksForMicroservices;
        _resourceRenderers = resourceRenderers;
        _applicationEnvironmentDeploymentLog = applicationEnvironmentDeploymentLog;
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
    public async Task Up(
        Application application,
        Func<Task> definition,
        ApplicationEnvironmentWithArtifacts environment,
        Microservice? microservice = default,
        UpOptions? upOptions = default,
        RefreshOptions? refreshOptions = default)
    {
        _logger.UppingStack();

        try
        {
            var stack = await CreateStack(application, environment, definition, microservice);
            await RefreshStack(stack.Stack, refreshOptions);

            _logger.PuttingUpStack();

            upOptions ??= new UpOptions
            {
                OnStandardOutput = Console.WriteLine,
                OnStandardError = Console.Error.WriteLine
            };
            await stack.Stack.UpAsync(upOptions);

            await (microservice is not null ?
                SaveStackForMicroservice(application, microservice, environment, stack.Stack) :
                SaveStackForApplication(application, environment, stack.Stack));
        }
        catch (Exception ex)
        {
            _logger.Errored(ex);

            throw;
        }
    }

    /// <inheritdoc/>
    public async Task Down(
        Application application,
        Func<Task> definition,
        ApplicationEnvironmentWithArtifacts environment,
        Microservice? microservice = default)
    {
        try
        {
            var stack = await CreateStack(application, environment, definition, microservice);
            await RefreshStack(stack.Stack);

            _logger.TakingDownStack();
            await stack.Stack.DestroyAsync(new DestroyOptions { OnStandardOutput = Console.WriteLine });

            await (microservice is not null ?
                SaveStackForMicroservice(application, microservice, environment, stack.Stack) :
                SaveStackForApplication(application, environment, stack.Stack));
        }
        catch (Exception ex)
        {
            _logger.Errored(ex);
        }
    }

    /// <inheritdoc/>
    public async Task Remove(
        Application application,
        Func<Task> definition,
        ApplicationEnvironmentWithArtifacts environment,
        Microservice? microservice = default)
    {
        _logger.StackBeingRemoved();

        try
        {
            var stack = await CreateStack(application, environment, definition, microservice);
            await RefreshStack(stack.Stack);

            _logger.RemovingStack();
            await stack.Stack.Workspace.RemoveStackAsync(environment.DisplayName);

            await (microservice is not null ?
                SaveStackForMicroservice(application, microservice, environment, stack.Stack) :
                SaveStackForApplication(application, environment, stack.Stack));
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
    public async Task ConsolidateEnvironment(
        Application application,
        ApplicationEnvironmentWithArtifacts environment,
        ApplicationEnvironmentDeploymentId deploymentId)
    {
        ApplicationEnvironmentResult? applicationEnvironmentResult = default;
        var executionContext = _executionContextManager.Current;
        var ingressResults = new Dictionary<Ingress, IngressResult>();
        Storage storage = null!;

        var upOptions = GetUpOptionsForDeployment(application, environment, deploymentId);
        var refreshOptions = GetRefreshOptionsForDeployment(application, environment, deploymentId);

        var sharedSubscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == application.Shared.AzureSubscriptionId);
        var sharedEnvironment = environment with
        {
            Id = PulumiStackDefinitions.SharedEnvironment.Id,
            Name = PulumiStackDefinitions.SharedEnvironment.Name,
            DisplayName = PulumiStackDefinitions.SharedEnvironment.DisplayName,
            ShortName = PulumiStackDefinitions.SharedEnvironment.ShortName,
            AzureSubscriptionId = sharedSubscription.SubscriptionId
        };

        var subscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == environment.AzureSubscriptionId);

        await Up(
            application,
            async () => await _stackDefinitions.Application(application, sharedEnvironment),
            sharedEnvironment,
            upOptions: upOptions,
            refreshOptions: refreshOptions);

        await Up(
            application,
            async () =>
            {
                applicationEnvironmentResult = await _stackDefinitions.ApplicationEnvironment(executionContext, application, environment, sharedEnvironment, environment.CratisVersion);
                environment = await applicationEnvironmentResult.MergeWithApplicationEnvironment(environment);
                storage = await application.GetStorage(environment, applicationEnvironmentResult!.ResourceGroup, _settings.ServicePrincipal, subscription);

                await _resourceRenderers.Render(
                    new(
                        application,
                        environment,
                        applicationEnvironmentResult.ResourceGroup,
                        application.GetTags(environment),
                        storage,
                        applicationEnvironmentResult.Network.VirtualNetwork),
                    environment.Resources);

                foreach (var ingress in environment.Ingresses)
                {
                    ingressResults[ingress] = await _stackDefinitions.Ingress(
                        executionContext,
                        application,
                        environment,
                        ingress,
                        applicationEnvironmentResult!.ManagedEnvironment,
                        applicationEnvironmentResult!.Certificates,
                        applicationEnvironmentResult!.ResourceGroup);
                }
            },
            environment,
            upOptions: upOptions,
            refreshOptions: refreshOptions);

        if (environment.ApplicationResources?.AzureResourceGroupId is null)
        {
            return;
        }

        var microserviceContainerApps = new Dictionary<MicroserviceId, ContainerApp>();
        foreach (var microservice in environment.Microservices)
        {
            await Up(
                application,
                async () =>
                {
                    var resourceGroup = application.GetResourceGroup(environment);
                    var managedEnvironment = await application.GetManagedEnvironment(environment, _settings.ServicePrincipal, subscription);
                    var microserviceResult = await HandleMicroservice(
                        executionContext,
                        application,
                        resourceGroup,
                        managedEnvironment,
                        microservice,
                        environment);
                    microserviceContainerApps[microservice.Id] = microserviceResult.ContainerApp;
                },
                environment,
                microservice,
                upOptions,
                refreshOptions);
        }

        foreach (var (ingress, result) in ingressResults)
        {
            await application.ConfigureIngress(
                environment,
                microserviceContainerApps,
                ingress,
                storage,
                result.FileShareName,
                _fileStorageLogger);
        }
    }

    /// <inheritdoc/>
    public async Task SetImageForDeployable(
        Application application,
        ApplicationEnvironmentWithArtifacts environment,
        Microservice microservice,
        ApplicationEnvironmentDeploymentId deploymentId,
        Deployable deployable,
        Concepts.Applications.DeployableImageName image)
    {
        var upOptions = GetUpOptionsForDeployment(application, environment, deploymentId);
        var refreshOptions = GetRefreshOptionsForDeployment(application, environment, deploymentId);
        var executionContext = _executionContextManager.Current;

        await Up(
            application,
            async () =>
            {
                var resourceGroup = application.GetResourceGroup(environment);
                var sharedEnvironment = environment with
                {
                    Id = PulumiStackDefinitions.SharedEnvironment.Id,
                    Name = PulumiStackDefinitions.SharedEnvironment.Name,
                    DisplayName = PulumiStackDefinitions.SharedEnvironment.DisplayName,
                    ShortName = PulumiStackDefinitions.SharedEnvironment.ShortName,
                };
                var sharedSubscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == application.Shared.AzureSubscriptionId);
                var containerRegistryResult = await application.GetContainerRegistry(sharedEnvironment, _settings.ServicePrincipal, sharedSubscription);
                var subscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == environment.AzureSubscriptionId);
                var managedEnvironment = await application.GetManagedEnvironment(environment, _settings.ServicePrincipal, subscription);

                environment = environment with
                {
                    ApplicationResources = new(
                        null!,
                        null!,
                        containerRegistryResult.LoginServer,
                        containerRegistryResult.UserName,
                        containerRegistryResult.Password,
                        null!,
                        null!),
                };

                var microserviceResult = await HandleMicroservice(
                    executionContext,
                    application,
                    resourceGroup,
                    managedEnvironment,
                    microservice,
                    environment);
            },
            environment,
            microservice,
            upOptions,
            refreshOptions);
    }

    async Task<PulumiStack> CreateStack(Application application, ApplicationEnvironmentWithArtifacts environment, Func<Task> program, Microservice? microservice = default)
    {
        _logger.CreatingStack(application.Name);
        var stackName = environment.DisplayName;

        var accessToken = _settings.PulumiAccessToken;
        _logger.PulumiInformation($"{accessToken.Value.Substring(0, 4)}*****");

        var mongoDBPublicKey = _settings.MongoDBPublicKey;
        var mongoDBPrivateKey = _settings.MongoDBPrivateKey;

        var projectName = GetProjectNameFor(application, microservice);

        PulumiStack? pulumiStack = null;

        var pulumiProgram = PulumiFn.Create(async () => await program());

        var args = new InlineProgramArgs(projectName, stackName, pulumiProgram)
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

        _logger.ExportStack();
        var stackDeployment = await stack.ExportStackAsync();
        var stackAsJson = JsonNode.Parse(stackDeployment.Json.GetRawText())!.AsObject();
        _logger.RemovingPendingOperations();

        await RemovePendingOperations(stack, stackAsJson);

        _logger.InstallingPlugins();
        await stack.Workspace.InstallPluginAsync("azure-native", "1.96.0");
        await stack.Workspace.InstallPluginAsync("azuread", "5.35.0");
        await stack.Workspace.InstallPluginAsync("mongodbatlas", "3.7.0");

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

        return pulumiStack = new(stack, stackAsJson);
    }

    async Task RemovePendingOperations(WorkspaceStack stack, JsonNode stackAsJson)
    {
        var deployment = stackAsJson!["deployment"]!.AsObject();
        if (deployment["pending_operations"] is not JsonArray pendingOperations || pendingOperations.Count == 0)
        {
            _logger.NoPendingOperations();
            return;
        }

        deployment.Remove("pending_operations");
        deployment.Add("pending_operations", new JsonArray());
        var jsonString = stackAsJson.ToJsonString(new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        var stackDeployment = StackDeployment.FromJsonString(jsonString);

        _logger.ImportStack();
        await stack.ImportStackAsync(stackDeployment);
    }

    async Task RefreshStack(WorkspaceStack stack, RefreshOptions? options = default)
    {
        options ??= new()
        {
            OnStandardOutput = Console.WriteLine,
            OnStandardError = Console.Error.WriteLine
        };

        _logger.RefreshingStack();
        await stack.RefreshAsync(options);
    }

    async Task SaveStackForApplication(Application application, Concepts.Applications.Environments.ApplicationEnvironment environment, WorkspaceStack stack)
    {
        _logger.SavingStackDeploymentForApplication(application.Name);

        var deployment = await stack.ExportStackAsync();
        await _stacksForApplications.Save(application.Id, environment, deployment);
    }

    async Task SaveStackForMicroservice(Application application, Microservice microservice, Concepts.Applications.Environments.ApplicationEnvironment environment, WorkspaceStack stack)
    {
        _logger.SavingStackDeploymentForMicroservice(microservice.Name);

        var deployment = await stack.ExportStackAsync();
        await _stacksForMicroservices.Save(application.Id, microservice.Id, environment, deployment);
    }

    async Task<ContainerAppResult> HandleMicroservice(
        ExecutionContext executionContext,
        Application application,
        ResourceGroup resourceGroup,
        ManagedEnvironment managedEnvironment,
        Microservice microservice,
        ApplicationEnvironmentWithArtifacts environment)
    {
        return await _stackDefinitions.Microservice(
            executionContext,
            application,
            microservice,
            resourceGroup,
            environment,
            managedEnvironment,
            true,
            deployables: microservice.Deployables);
    }

    RefreshOptions GetRefreshOptionsForDeployment(
        Application application,
        ApplicationEnvironmentWithArtifacts environment,
        ApplicationEnvironmentDeploymentId deploymentId)
    {
        var options = new RefreshOptions();
        SetOptionsForDeployment(application, environment, deploymentId, options);
        return options;
    }

    UpOptions GetUpOptionsForDeployment(
        Application application,
        ApplicationEnvironmentWithArtifacts environment,
        ApplicationEnvironmentDeploymentId deploymentId)
    {
        var options = new UpOptions();
        SetOptionsForDeployment(application, environment, deploymentId, options);
        return options;
    }

    void SetOptionsForDeployment(
        Application application,
        ApplicationEnvironmentWithArtifacts environment,
        ApplicationEnvironmentDeploymentId deploymentId,
        global::Pulumi.Automation.UpdateOptions options)
    {
        options.OnStandardOutput = (message) =>
            {
                _applicationEnvironmentDeploymentLog.Append(application.Id, environment.Id, deploymentId, message);
                Console.WriteLine(message);
            };

        options.OnStandardError = (message) =>
            {
                _applicationEnvironmentDeploymentLog.Append(application.Id, environment.Id, deploymentId, message);
                Console.WriteLine(message);
            };
    }
}
