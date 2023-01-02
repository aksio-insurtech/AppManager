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
using MicroserviceId = Concepts.Applications.MicroserviceId;

namespace Reactions.Applications.Pulumi;

#pragma warning disable RCS1175, IDE0042

public class PulumiStackDefinitions : IPulumiStackDefinitions
{
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

    public async Task<ApplicationEnvironmentResult> ApplicationEnvironment(ExecutionContext executionContext, Application application, ApplicationEnvironmentWithArtifacts environment, SemanticVersion cratisVersion)
    {
        var tags = application.GetTags(environment);
        var subscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == environment.AzureSubscriptionId);
        var resourceGroup = await application.SetupResourceGroup(environment, _settings.ServicePrincipal, subscription);
        var identity = application.SetupUserAssignedIdentity(environment, resourceGroup, tags);
        var vault = application.SetupKeyVault(environment, identity, resourceGroup, tags);
        var network = application.SetupNetwork(environment, identity, vault, resourceGroup, tags);
        var storage = await application.SetupStorage(environment, resourceGroup, tags);
        var containerRegistry = await application.SetupContainerRegistry(resourceGroup, tags);
        var mongoDB = await application.SetupMongoDB(_settings, resourceGroup, network.VirtualNetwork, environment, tags);

        var microservice = new Microservice(
            Guid.Empty,
            "kernel",
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
        kernelStorage.CreateAndUploadAppSettings(_settings);

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
        var storage = await application.GetStorage(environment, resourceGroup);

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
        ApplicationEnvironmentWithArtifacts environment,
        ManagedEnvironment managedEnvironment,
        bool useContainerRegistry = true,
        ResourceGroup? resourceGroup = default,
        IEnumerable<Deployable>? deployables = default)
    {
        var tags = application.GetTags(environment);
        resourceGroup ??= application.GetResourceGroup(environment);
        var storage = await microservice.GetStorage(application, environment, resourceGroup, _fileStorageLogger);
        storage.CreateAndUploadAppSettings(_settings);
        storage.CreateAndUploadClusterClientConfig(storage.FileStorage.ConnectionString);

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

        // Todo: Set to actual execution context - might not be the right place for this!
        _executionContextManager.Set(executionContext);

        return microserviceResult;
    }

    public async Task Deployable(
        ExecutionContext executionContext,
        Application application,
        Microservice microservice,
        IEnumerable<Deployable> deployables,
        ManagedEnvironment managedEnvironment,
        ApplicationEnvironmentWithArtifacts environment)
    {
        var tags = application.GetTags(environment);
        var resourceGroup = application.GetResourceGroup(environment);
        var storage = await microservice.GetStorage(application, environment, resourceGroup, _fileStorageLogger);

        _ = microservice.SetupContainerApp(
            application,
            resourceGroup,
            managedEnvironment,
            environment.ApplicationResources.AzureContainerRegistryLoginServer,
            environment.ApplicationResources.AzureContainerRegistryUserName,
            environment.ApplicationResources.AzureContainerRegistryPassword,
            storage,
            deployables,
            tags);

        // Todo: Set to actual execution context - might not be the right place for this!
        _executionContextManager.Set(executionContext);
    }
}
