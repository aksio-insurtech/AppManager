// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.Execution;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.AppContainers;
using Common;
using Concepts.Applications.Environments;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Pulumi.Automation;
using Pulumi.AzureNative.App;
using Pulumi.AzureNative.Resources;
using Reactions.Applications.Pulumi.Resources;
using Reactions.Applications.Pulumi.Resources.Cratis;
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
    readonly IResourceRendering _resourceRenderers;
    readonly IApplicationEnvironmentDeploymentLog _applicationEnvironmentDeploymentLog;

    public PulumiOperations(
        ISettings settings,
        IExecutionContextManager executionContextManager,
        IPulumiStackDefinitions stackDefinitions,
        IStacksForApplications stacksForApplications,
        IStacksForMicroservices stacksForMicroservices,
        IResourceRendering resourceRenderers,
        IApplicationEnvironmentDeploymentLog applicationEnvironmentDeploymentLog,
        ILogger<PulumiOperations> logger,
        ILogger<FileStorage> fileStorageLogger)
    {
        _logger = logger;
        _fileStorageLogger = fileStorageLogger;
        _settings = settings;
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

            // await stack.Stack.PreviewAsync(new PreviewOptions
            // {
            //     OnStandardOutput = upOptions.OnStandardOutput,
            //     OnStandardError = upOptions.OnStandardError,
            //     Json = true,
            //     Diff = true,
            // });
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
        using var client = new HttpClient();
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
        var resourceScope = new ResourceRenderingScope(environment.Resources);

        var sharedSubscription =
            _settings.AzureSubscriptions.First(_ => _.SubscriptionId == application.Shared.AzureSubscriptionId);
        var sharedEnvironment = environment with
        {
            Id = PulumiStackDefinitions.SharedEnvironment.Id,
            Name = PulumiStackDefinitions.SharedEnvironment.Name,
            DisplayName = PulumiStackDefinitions.SharedEnvironment.DisplayName,
            ShortName = PulumiStackDefinitions.SharedEnvironment.ShortName,
            AzureSubscriptionId = sharedSubscription.SubscriptionId
        };

        var subscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == environment.AzureSubscriptionId);
        var results = new ResourceResultsByType();

        async Task<RenderContext> RenderResources(ResourceLevel level, ResourceGroup resourceGroup)
        {
            var context = new RenderContext(
                executionContext,
                _settings,
                application,
                environment,
                resourceGroup,
                application.GetTags(environment),
                results,
                environment.Tenants,
                environment.Microservices);

            await _resourceRenderers.Render(level, context, resourceScope);

            return context;
        }

        RenderContext? sharedContext = default;

        await Up(
            application,
            async () =>
            {
                var resourceGroup = await _stackDefinitions.Application(application, sharedEnvironment, results);
                sharedContext = await RenderResources(ResourceLevel.Shared, resourceGroup);
            },
            sharedEnvironment,
            upOptions: upOptions,
            refreshOptions: refreshOptions);

        await Up(
            application,
            async () =>
            {
                applicationEnvironmentResult = await _stackDefinitions.ApplicationEnvironment(
                    executionContext,
                    application,
                    environment,
                    sharedEnvironment,
                    results);
                environment = await applicationEnvironmentResult.MergeWithApplicationEnvironment(environment);
                storage = await application.GetStorage(environment, _settings.ServicePrincipal, subscription);
                await RenderResources(ResourceLevel.Environment, applicationEnvironmentResult.ResourceGroup);

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

        var microserviceContainerApps = new Dictionary<MicroserviceId, ContainerApp>();
        foreach (var microservice in environment.Microservices)
        {
            await Up(
                application,
                async () =>
                {
                    var resourceGroup = application.GetResourceGroup(environment);
                    var managedEnvironment = await application.GetManagedEnvironment(
                        environment,
                        _settings.ServicePrincipal,
                        subscription);
                    var microserviceResult = await HandleMicroservice(
                        executionContext,
                        application,
                        resourceGroup,
                        managedEnvironment,
                        microservice,
                        environment,
                        results);
                    microserviceContainerApps[microservice.Id] = microserviceResult.ContainerApp;
                },
                environment,
                microservice,
                upOptions,
                refreshOptions);
        }

        if (sharedContext!.Results.TryGetValue(CratisResourceRenderer.ResourceTypeId, out var sharedCratisResult))
        {
            microserviceContainerApps[CratisResourceRenderer.ResourceTypeId] = (sharedCratisResult as ContainerApp)!;
        }

        foreach (var (ingress, result) in ingressResults)
        {
            var configChanged = await application.ConfigureIngress(
                environment,
                microserviceContainerApps,
                ingress,
                storage,
                result.FileShareName,
                _fileStorageLogger);

            // And finally restart the ingress, this must be done after uploading the configfiles.
            if (configChanged)
            {
                await RestartIngress(application, environment, result.ContainerApp);
            }
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
                var results = new ResourceResultsByType();

                var sharedSubscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == application.Shared.AzureSubscriptionId);

                var containerRegistryResult = await application.GetContainerRegistry(sharedEnvironment, _settings.ServicePrincipal, sharedSubscription);
                if (containerRegistryResult is not null)
                {
                    results.Register(WellKnownResourceTypes.ContainerRegistry, containerRegistryResult);
                }

                var subscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == environment.AzureSubscriptionId);
                var managedEnvironment = await application.GetManagedEnvironment(environment, _settings.ServicePrincipal, subscription);

                var microserviceResult = await HandleMicroservice(
                    executionContext,
                    application,
                    resourceGroup,
                    managedEnvironment,
                    microservice,
                    environment,
                    results);
            },
            environment,
            microservice,
            upOptions,
            refreshOptions);
    }

    /// <summary>
    /// Restart all revisions for an ingress, this is necessary after the config is updated (which we do after provisioning the actual ingress container).
    /// </summary>
    /// <param name="application">Application the environment is for.</param>
    /// <param name="environment">The environment to consolidate.</param>
    /// <param name="containerApp">The ingress container app.</param>
    async Task RestartIngress(
        Application application,
        ApplicationEnvironmentWithArtifacts environment,
        ContainerApp containerApp)
    {
        var subscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == environment.AzureSubscriptionId);

        var cred = new ClientSecretCredential(
            subscription.TenantId,
            _settings.ServicePrincipal.ClientId,
            _settings.ServicePrincipal.ClientSecret);

        string subscriptionId = subscription.SubscriptionId;
        var resourceGroupName = application.GetResourceGroupName(environment, environment.CloudLocation);
        var containerAppName = await containerApp.Name.GetValue();
        var revisionName = await containerApp.LatestRevisionName.GetValue();

        _logger.RestartingIngressRevision(containerAppName, revisionName);

        var client = new ArmClient(cred);
        var containerAppResourceId =
            ContainerAppResource.CreateResourceIdentifier(subscriptionId, resourceGroupName, containerAppName);
        var containerAppResource = client.GetContainerAppResource(containerAppResourceId);

        var revisions = containerAppResource.GetContainerAppRevisions();
        await foreach (var revision in revisions.GetAllAsync())
        {
            if (revision.Id.Name == revisionName)
            {
                await revision.RestartRevisionAsync();
            }
        }
    }

    async Task<PulumiStack> CreateStack(Application application, ApplicationEnvironmentWithArtifacts environment, Func<Task> program, Microservice? microservice = default)
    {
        _logger.CreatingStack(application.Name);
        var stackName = $"{_settings.PulumiOrganization}/{environment.DisplayName}";

        var accessToken = _settings.PulumiAccessToken;
        _logger.PulumiInformation($"{accessToken.Value.Substring(0, 4)}*****");

        var mongoDBPublicKey = _settings.MongoDBPublicKey;
        var mongoDBPrivateKey = _settings.MongoDBPrivateKey;

        var projectName = GetProjectNameFor(application, microservice);

        var pulumiProgram = PulumiFn.Create(async () => await program());
        var args = new InlineProgramArgs(projectName, stackName, pulumiProgram)
        {
            ProjectSettings = new(projectName, ProjectRuntimeName.Dotnet),
            EnvironmentVariables = new Dictionary<string, string?>
            {
                { "TF_LOG", "TRACE" },
                { "ARM_CLIENT_ID", _settings.ServicePrincipal.ClientId },
                { "ARM_CLIENT_SECRET", _settings.ServicePrincipal.ClientSecret },
                { "ARM_TENANT_ID", _settings.AzureSubscriptions.First().TenantId },
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
        await stack.Workspace.InstallPluginAsync("azure-native", "2.19.0");
        await stack.Workspace.InstallPluginAsync("azuread", "5.45.0");
        await stack.Workspace.InstallPluginAsync("mongodbatlas", "3.12.1");

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
        await SetTag(application, environment, "environment", environment.DisplayName);

        return new(stack, stackAsJson);
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

        // If refresh fails after a little while with "AADSTS7000215: Invalid client secret provided." after an update to the Entra ID service principal, you need to manually set the secret.
        // This is described in the solution README.md.
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
        ApplicationEnvironmentWithArtifacts environment,
        ResourceResultsByType results)
    {
        return await _stackDefinitions.Microservice(
            executionContext,
            application,
            microservice,
            resourceGroup,
            environment,
            managedEnvironment,
            results,
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
