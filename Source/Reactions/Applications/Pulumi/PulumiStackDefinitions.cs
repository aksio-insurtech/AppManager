// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Common;
using Concepts;
using Microsoft.Extensions.Logging;
using Pulumi.Automation;

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

    public PulumiFn Application(ExecutionContext executionContext, Application application, CloudRuntimeEnvironment environment) =>
        PulumiFn.Create(async () =>
        {
            var tags = application.GetTags(environment);
            var resourceGroup = application.SetupResourceGroup();
            var identity = application.SetupUserAssignedIdentity(resourceGroup, tags);
            var vault = application.SetupKeyVault(identity, resourceGroup, tags);
            var network = application.SetupNetwork(identity, vault, resourceGroup, tags);
            var storage = await application.SetupStorage(resourceGroup, tags);
            var containerRegistry = await application.SetupContainerRegistry(resourceGroup, tags);
            var mongoDB = await application.SetupMongoDB(_settings, environment, tags);

            var microservice = new Microservice(Guid.Empty, application.Id, "kernel");
            var fileStorage = new FileStorage(storage.AccountName, storage.AccountKey, storage.FileShare, _fileStorageLogger);
            var kernelStorage = new MicroserviceStorage(application, microservice, fileStorage);

            var networkProfile = await network.Profile.Id.GetValue();

            var applicationInsights = application.SetupApplicationInsights(resourceGroup, environment, tags);
            var managedEnvironment = application.SetupContainerAppManagedEnvironment(resourceGroup, environment, applicationInsights, tags);
            var managedEnvironmentId = await managedEnvironment.Id.GetValue();
            var managedEnvironmentName = await managedEnvironment.Name.GetValue();

            var kernel = await microservice.SetupContainerApp(
                application,
                resourceGroup,
                networkProfile,
                managedEnvironmentId,
                managedEnvironmentName,
                containerRegistry.LoginServer,
                containerRegistry.UserName,
                containerRegistry.Password,
                kernelStorage,
                new[]
                {
                    new Deployable(Guid.Empty, microservice.Id, "kernel", "aksioinsurtech/cratis:6.1.10", new[] { 80 })
                },
                tags);

            kernelStorage.CreateAndUploadCratisJson(mongoDB, kernel.SiloHostName, fileStorage.ConnectionString);
            kernelStorage.CreateAndUploadAppSettings(_settings);

            Console.WriteLine(_eventLog);

            await application.SetupIngress(resourceGroup, storage, managedEnvironment, tags, _fileStorageLogger);

            // var applicationResult = new ApplicationResult(
            //     environment,
            //     resourceGroup,
            //     network,
            //     storage,
            //     containerRegistry,
            //     mongoDB,
            //     kernel);
            // var events = await application.GetEventsToAppend(applicationResult);
            // // Todo: Set to actual execution context - might not be the right place for this!
            // _executionContextManager.Set(executionContext);
            // foreach (var @event in events)
            // {
            //     await _eventLog.Append(application.Id, @event);
            // }
        });

    public PulumiFn Microservice(ExecutionContext executionContext, Application application, Microservice microservice, CloudRuntimeEnvironment environment) =>
        PulumiFn.Create(async () =>
        {
            var tags = application.GetTags(environment);
            var resourceGroup = application.SetupResourceGroup();
            var storage = await microservice.GetStorage(application, resourceGroup, _fileStorageLogger);
            storage.CreateAndUploadAppSettings(_settings);
            storage.CreateAndUploadClusterClientConfig(storage.FileStorage.ConnectionString);

            var managedEnvironment = await application.GetContainerAppManagedEnvironment(resourceGroup, environment);

            var microserviceResult = microservice.SetupContainerApp(
                application,
                resourceGroup,
                application.Resources.AzureNetworkProfileIdentifier,
                managedEnvironment.Id,
                managedEnvironment.Name,
                application.Resources.AzureContainerRegistryLoginServer,
                application.Resources.AzureContainerRegistryUserName,
                application.Resources.AzureContainerRegistryPassword,
                storage,
                new[]
                {
                    new Deployable(Guid.Empty, microservice.Id, "main", $"{application.Resources.AzureContainerRegistryLoginServer}/members:1.2.12", new[] { 80 }),
                    new Deployable(Guid.Empty, microservice.Id, "fto", $"{application.Resources.AzureContainerRegistryLoginServer}/ftoapi:1.1.8", new[] { 5003 })
                },
                tags);

            // Todo: Set to actual execution context - might not be the right place for this!
            _executionContextManager.Set(executionContext);
        });

    public PulumiFn Deployable(ExecutionContext executionContext, Application application, Microservice microservice, Deployable deployable, CloudRuntimeEnvironment environment) =>
        PulumiFn.Create(async () =>
        {
            var tags = application.GetTags(environment);
            var resourceGroup = application.SetupResourceGroup();
            var storage = await microservice.GetStorage(application, resourceGroup, _fileStorageLogger);

            var managedEnvironment = await application.GetContainerAppManagedEnvironment(resourceGroup, environment);

            _ = microservice.SetupContainerApp(
                application,
                resourceGroup,
                application.Resources.AzureNetworkProfileIdentifier,
                managedEnvironment.Id,
                managedEnvironment.Name,
                application.Resources.AzureContainerRegistryLoginServer,
                application.Resources.AzureContainerRegistryUserName,
                application.Resources.AzureContainerRegistryPassword,
                storage,
                new[]
                {
                    deployable
                },
                tags);

            // Todo: Set to actual execution context - might not be the right place for this!
            _executionContextManager.Set(executionContext);
        });
}
