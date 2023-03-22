// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Common;
using Concepts;
using Concepts.Applications;
using Concepts.Applications.Environments;
using Microsoft.Extensions.Logging;
using Pulumi;
using Pulumi.AzureNative.App;
using Pulumi.AzureNative.Resources;
using Reactions.Applications.Templates;
using MicroserviceId = Concepts.Applications.MicroserviceId;

namespace Reactions.Applications.Pulumi;

#pragma warning disable RCS1175, IDE0042

public class PulumiStackDefinitions : IPulumiStackDefinitions
{
    public static readonly ApplicationEnvironment SharedEnvironment = new(
        Guid.Parse("5e4d2c45-4ebb-4bc4-a178-b1d1d5ed44ff"),
        "Shared",
        "shared",
        "S");

    readonly ISettings _settings;
    readonly IExecutionContextManager _executionContextManager;
    readonly IEventLog _eventLog;
    readonly ILogger<FileStorage> _fileStorageLogger;

    public PulumiStackDefinitions(
        ISettings applicationSettings,
        IExecutionContextManager executionContextManager,
        IEventLog eventLog,
        ILogger<FileStorage> fileStorageLogger)
    {
        _settings = applicationSettings;
        _executionContextManager = executionContextManager;
        _eventLog = eventLog;
        _fileStorageLogger = fileStorageLogger;
    }

    public Task Application(Application application, ApplicationEnvironmentWithArtifacts sharedEnvironment)
    {
        var sharedTags = application.GetTags(sharedEnvironment);
        var sharedResourceGroup = application.SetupResourceGroup(sharedEnvironment);
        application.SetupContainerRegistry(sharedResourceGroup, sharedTags);

        return Task.CompletedTask;
    }

    public async Task<ApplicationEnvironmentResult> ApplicationEnvironment(
        ExecutionContext executionContext,
        Application application,
        ApplicationEnvironmentWithArtifacts environment,
        ApplicationEnvironmentWithArtifacts sharedEnvironment,
        SemanticVersion cratisVersion)
    {
        var sharedSubscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == sharedEnvironment.AzureSubscriptionId);
        var containerRegistry = await application.GetContainerRegistry(sharedEnvironment, _settings.ServicePrincipal, sharedSubscription);

        var tags = application.GetTags(environment);
        var subscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == environment.AzureSubscriptionId);
        var resourceGroup = application.SetupResourceGroup(environment);
        var identity = application.SetupUserAssignedIdentity(environment, resourceGroup, tags);
        var vault = application.SetupKeyVault(environment, identity, resourceGroup, tags);
        var network = application.SetupNetwork(environment, identity, vault, resourceGroup, tags);
        var storage = await application.SetupStorage(environment, resourceGroup, tags);
        var mongoDB = await application.SetupMongoDB(_settings, resourceGroup, network.VirtualNetwork, environment, tags);

        var microservice = new Microservice(
            Guid.Empty,
            "kernel",
            AppSettingsContent.Empty,
            new Deployable[]
            {
                new Deployable(Guid.Empty, Guid.Empty, "kernel", $"docker.io/aksioinsurtech/cratis:{cratisVersion}", new[] { 80 }, ConfigPath.Default)
            },
            Enumerable.Empty<MicroserviceId>());

        var fileStorage = new FileStorage(storage.AccountName, storage.AccountKey, storage.FileShare, _fileStorageLogger);
        var kernelStorage = new MicroserviceStorage(application, microservice, fileStorage);

        var applicationMonitoring = application.SetupApplicationMonitoring(resourceGroup, environment, tags);
        var managedEnvironment = application.SetupContainerAppManagedEnvironment(resourceGroup, environment, applicationMonitoring.Workspace, network, tags);

        var certificates = application.SetupCertificates(environment, managedEnvironment, resourceGroup, tags);

        var kernel = await microservice.SetupContainerApp(
            application,
            resourceGroup,
            managedEnvironment,
            containerRegistry.LoginServer,
            containerRegistry.UserName,
            containerRegistry.Password,
            kernelStorage,
            microservice.Deployables,
            tags,
            false);

        await kernelStorage.CreateAndUploadCratisJson(mongoDB, environment.Tenants, environment.Microservices, kernel.SiloHostName, fileStorage.ConnectionString, applicationMonitoring);

        var content = TemplateTypes.AppSettings(null!);
        fileStorage.Upload("appsettings.json", content);

        var applicationResult = new ApplicationEnvironmentResult(
            environment,
            resourceGroup,
            network,
            storage,
            containerRegistry,
            mongoDB,
            managedEnvironment,
            certificates,
            kernel);
        var events = await application.GetEventsToAppend(environment, applicationResult);

        // Todo: Set to actual execution context - might not be the right place for this!
        _executionContextManager.Set(executionContext);
        foreach (var @event in events)
        {
            await _eventLog.Append(environment.Id, @event);
        }

        return applicationResult;
    }

    public async Task<IngressResult> Ingress(
        ExecutionContext executionContext,
        Application application,
        ApplicationEnvironmentWithArtifacts environment,
        Ingress ingress,
        ManagedEnvironment managedEnvironment,
        IDictionary<CertificateId, Output<string>> certificates,
        ResourceGroup? resourceGroup = default)
    {
        var tags = application.GetTags(environment);
        resourceGroup ??= application.GetResourceGroup(environment);
        var subscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == environment.AzureSubscriptionId);
        var storage = await application.GetStorage(environment, _settings.ServicePrincipal, subscription);

        return await application.SetupIngress(
            environment,
            resourceGroup,
            storage,
            managedEnvironment,
            certificates,
            ingress,
            new Dictionary<MicroserviceId, ContainerApp>(),
            tags,
            _fileStorageLogger);
    }

    public async Task<ContainerAppResult> Microservice(
        ExecutionContext executionContext,
        Application application,
        Microservice microservice,
        ResourceGroup resourceGroup,
        ApplicationEnvironmentWithArtifacts environment,
        ManagedEnvironment managedEnvironment,
        bool useContainerRegistry = true,
        IEnumerable<Deployable>? deployables = default)
    {
        var tags = application.GetTags(environment);
        var subscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == environment.AzureSubscriptionId);
        var storage = await microservice.SetupFileShare(application, environment, resourceGroup, _settings.ServicePrincipal, subscription, _fileStorageLogger);
        storage.CreateAndUploadAppSettings();

        deployables ??= Array.Empty<Deployable>();

        var microserviceResult = await microservice.SetupContainerApp(
            application,
            resourceGroup,
            managedEnvironment,
            environment.ApplicationResources.AzureContainerRegistryLoginServer,
            environment.ApplicationResources.AzureContainerRegistryUserName,
            environment.ApplicationResources.AzureContainerRegistryPassword,
            storage,
            deployables,
            tags,
            useContainerRegistry);

        microserviceResult.ContainerApp.Configuration.Apply(_ => _?.Ingress).Apply(_ =>
        {
            var url = $"http://{_?.Fqdn}";
            storage.CreateAndUploadClientCratisConfig(storage.FileStorage.ConnectionString, url);
            return _?.Fqdn;
        });

        // Todo: Set to actual execution context - might not be the right place for this!
        _executionContextManager.Set(executionContext);

        return microserviceResult;
    }
}
