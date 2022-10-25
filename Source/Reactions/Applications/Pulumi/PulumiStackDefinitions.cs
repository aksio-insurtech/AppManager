// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Common;
using Concepts;
using Concepts.Applications.Environments;
using Microsoft.Extensions.Logging;
using Pulumi.AzureNative.Resources;

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

    public async Task<ApplicationResult> ApplicationEnvironment(ExecutionContext executionContext, Application application, ApplicationEnvironment environment, SemanticVersion cratisVersion)
    {
        var tags = application.GetTags(environment);
        var resourceGroup = application.SetupResourceGroup(environment);
        var identity = application.SetupUserAssignedIdentity(resourceGroup, tags);
        var vault = application.SetupKeyVault(identity, resourceGroup, tags);
        var network = application.SetupNetwork(identity, vault, resourceGroup, tags);
        var storage = await application.SetupStorage(environment, resourceGroup, tags);
        var containerRegistry = await application.SetupContainerRegistry(resourceGroup, tags);
        var mongoDB = await application.SetupMongoDB(_settings, resourceGroup, network.VirtualNetwork, environment, tags);

        var microservice = new Microservice(Guid.Empty, application.Id, "kernel", new Deployable[]
        {
            new Deployable(Guid.Empty, Guid.Empty, "kernel", $"docker.io/aksioinsurtech/cratis:{cratisVersion}", new[] { 80 })
        });
        var fileStorage = new FileStorage(storage.AccountName, storage.AccountKey, storage.FileShare, _fileStorageLogger);
        var kernelStorage = new MicroserviceStorage(application, microservice, fileStorage);

        var applicationMonitoring = application.SetupApplicationMonitoring(resourceGroup, environment, tags);
        var managedEnvironment = application.SetupContainerAppManagedEnvironment(resourceGroup, environment, applicationMonitoring.Workspace, network, tags);

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

        await kernelStorage.CreateAndUploadCratisJson(mongoDB, kernel.SiloHostName, fileStorage.ConnectionString, applicationMonitoring);
        kernelStorage.CreateAndUploadAppSettings(_settings);

        var applicationResult = new ApplicationResult(
            environment,
            resourceGroup,
            network,
            storage,
            containerRegistry,
            mongoDB,
            managedEnvironment,
            kernel);
        var events = await application.GetEventsToAppend(applicationResult);

        // Todo: Set to actual execution context - might not be the right place for this!
        _executionContextManager.Set(executionContext);
        foreach (var @event in events)
        {
            await _eventLog.Append(application.Id, @event);
        }

        return applicationResult;
    }

    public async Task<IngressResult> Ingress(ExecutionContext executionContext, Application application, ApplicationEnvironment environment, Ingress ingress)
    {
        var tags = application.GetTags(environment);
        var resourceGroup = application.GetResourceGroup(environment);
        var managedEnvironment = await application.GetContainerAppManagedEnvironment(resourceGroup, environment);
        var storage = await application.GetStorage(environment, resourceGroup);

        return await application.SetupIngress(resourceGroup, storage, managedEnvironment, tags, _fileStorageLogger);
    }

    public async Task<ContainerAppResult> Microservice(
        ExecutionContext executionContext,
        Application application,
        Microservice microservice,
        ApplicationEnvironment environment,
        bool useContainerRegistry = true,
        ResourceGroup? resourceGroup = default,
        IEnumerable<Deployable>? deployables = default)
    {
        var tags = application.GetTags(environment);
        resourceGroup ??= application.GetResourceGroup(environment);
        var storage = await microservice.GetStorage(application, resourceGroup, _fileStorageLogger);
        storage.CreateAndUploadAppSettings(_settings);
        storage.CreateAndUploadClusterClientConfig(storage.FileStorage.ConnectionString);

        var managedEnvironment = await application.GetContainerAppManagedEnvironment(resourceGroup, environment);

        deployables ??= Array.Empty<Deployable>();

        var microserviceResult = await microservice.SetupContainerApp(
            application,
            resourceGroup,
            managedEnvironment,
            application.Resources.AzureContainerRegistryLoginServer,
            application.Resources.AzureContainerRegistryUserName,
            application.Resources.AzureContainerRegistryPassword,
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
        ApplicationEnvironment environment)
    {
        var tags = application.GetTags(environment);
        var resourceGroup = application.GetResourceGroup(environment);
        var storage = await microservice.GetStorage(application, resourceGroup, _fileStorageLogger);

        var managedEnvironment = await application.GetContainerAppManagedEnvironment(resourceGroup, environment);

        _ = microservice.SetupContainerApp(
            application,
            resourceGroup,
            managedEnvironment,
            application.Resources.AzureContainerRegistryLoginServer,
            application.Resources.AzureContainerRegistryUserName,
            application.Resources.AzureContainerRegistryPassword,
            storage,
            deployables,
            tags);

        // Todo: Set to actual execution context - might not be the right place for this!
        _executionContextManager.Set(executionContext);
    }
}
