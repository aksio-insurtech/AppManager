// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Common;
using Concepts;
using Microsoft.Extensions.Logging;
using Pulumi;
using Pulumi.Automation;

namespace Reactions.Applications.Pulumi;

public class PulumiStackDefinitions : IPulumiStackDefinitions
{
    readonly ISettings _settings;
    readonly IExecutionContextManager _executionContextManager;
    readonly IEventLog _eventLog;
    readonly ILogger<MicroserviceStorage> _microserviceStorageLogger;

    public PulumiStackDefinitions(
        ISettings applicationSettings,
        IExecutionContextManager executionContextManager,
        IEventLog eventLog,
        ILogger<MicroserviceStorage> microserviceStorageLogger)
    {
        _settings = applicationSettings;
        _executionContextManager = executionContextManager;
        _eventLog = eventLog;
        _microserviceStorageLogger = microserviceStorageLogger;
    }

    public PulumiFn Application(ExecutionContext executionContext, Application application, CloudRuntimeEnvironment environment) =>

        PulumiFn.Create(async () =>
        {
            var tags = application.GetTags(environment);
            var resourceGroup = application.SetupResourceGroup();
            var network = application.SetupNetwork(resourceGroup, tags);
            var storage = await application.SetupStorage(resourceGroup, tags);
            var containerRegistry = await application.SetupContainerRegistry(resourceGroup, tags);
            var mongoDB = await application.SetupMongoDB(_settings, environment, tags);

            var microservice = new Microservice(Guid.Empty, application.Id, "kernel");
            var kernelStorage = new MicroserviceStorage(application, microservice, storage.AccountName, storage.AccountKey, storage.FileShare, _microserviceStorageLogger);

            kernelStorage.CreateAndUploadStorageJson(mongoDB.ConnectionString);
            kernelStorage.CreateAndUploadAppSettings(_settings);

            var networkProfile = await network.Profile.Id.GetValue();

            application.SetupIngress(resourceGroup, networkProfile, storage, tags);
            var kernelContainer = microservice.SetupContainerGroup(
                application,
                resourceGroup,
                networkProfile,
                kernelStorage,
                new[]
                {
                    new Deployable(Guid.Empty, microservice.Id, "kernel", "aksioinsurtech/cratis:5.11.1")
                },
                tags);

            var applicationResult = new ApplicationResult(
                environment,
                resourceGroup,
                network,
                storage,
                containerRegistry,
                mongoDB,
                kernelContainer);

            var events = await application.GetEventsToAppend(applicationResult);

            // Todo: Set to actual execution context - might not be the right place for this!
            _executionContextManager.Set(executionContext);
            foreach (var @event in events)
            {
                await _eventLog.Append(application.Id, @event);
            }
        });

    public PulumiFn Microservice(ExecutionContext executionContext, Application application, Microservice microservice, CloudRuntimeEnvironment environment) =>
        PulumiFn.Create(async () =>
        {
            // Todo: Set to actual tenant - and probably not here!
            _executionContextManager.Establish(TenantId.Development, CorrelationId.New());

            var tags = application.GetTags(environment);
            var resourceGroup = application.SetupResourceGroup();
            var storage = await microservice.GetStorage(application, resourceGroup, _microserviceStorageLogger);
            storage.CreateAndUploadAppSettings(_settings);

            Log.Info($"NetworkProfile : {application.Resources.AzureNetworkProfileIdentifier}");

            _ = microservice.SetupContainerGroup(
                application,
                resourceGroup,
                application.Resources.AzureNetworkProfileIdentifier,
                storage,
                new[]
                {
                    new Deployable(Guid.Empty, microservice.Id, "main", "nginx")
                },
                tags);
        });

    public PulumiFn Deployable(ExecutionContext executionContext, Application application, Microservice microservice, Deployable deployable, CloudRuntimeEnvironment environment) =>
        PulumiFn.Create(async () =>
        {
            // Todo: Set to actual tenant - and probably not here!
            _executionContextManager.Establish(TenantId.Development, CorrelationId.New());

            var tags = application.GetTags(environment);
            var resourceGroup = application.SetupResourceGroup();
            var storage = await microservice.GetStorage(application, resourceGroup, _microserviceStorageLogger);

            _ = microservice.SetupContainerGroup(
                application,
                resourceGroup,
                application.Resources.AzureNetworkProfileIdentifier,
                storage,
                new[]
                {
                    deployable
                },
                tags);
        });
}
