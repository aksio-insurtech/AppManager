// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Common;
using Events.Applications.Environments;
using Events.Applications.Environments.Microservices;
using Events.Applications.Environments.Microservices.Modules.Deployables;
using Microsoft.Extensions.Logging;
using Reactions.Applications.Pulumi;

namespace Reactions.Applications;

[Observer("c2f0e081-6a1a-46e0-bc8c-8a08e0c4dff5")]
public class ApplicationEnvironmentResourcesCoordinator
{
    readonly ILogger<ApplicationEnvironmentResourcesCoordinator> _logger;
    readonly ILogger<FileStorage> _fileStorageLogger;
    readonly IImmediateProjections _projections;
    readonly ExecutionContext _executionContext;
    readonly IExecutionContextManager _executionContextManager;
    readonly IPulumiOperations _pulumiOperations;
    readonly IEventLog _eventLog;
    readonly ISettings _settings;

    public ApplicationEnvironmentResourcesCoordinator(
        IImmediateProjections projections,
        ExecutionContext executionContext,
        IExecutionContextManager executionContextManager,
        IPulumiStackDefinitions stackDefinitions,
        IPulumiOperations pulumiOperations,
        IEventLog eventLog,
        ISettings settings,
        ILogger<ApplicationEnvironmentResourcesCoordinator> logger,
        ILogger<FileStorage> fileStorageLogger)
    {
        _logger = logger;
        _fileStorageLogger = fileStorageLogger;
        _projections = projections;
        _executionContext = executionContext;
        _executionContextManager = executionContextManager;
        _pulumiOperations = pulumiOperations;
        _eventLog = eventLog;
        _settings = settings;
    }

    public Task DeploymentStarted(ApplicationEnvironmentDeploymentStarted @event, EventContext context)
    {
        _ = Task.Run(async () =>
        {
            var application = await _projections.GetInstanceById<Application>(@event.ApplicationId);
            var environment = await _projections.GetInstanceById<ApplicationEnvironmentWithArtifacts>(context.EventSourceId);

            _logger.DeploymentStarted(environment.Model.Name, application.Model.Name);
            try
            {
                await _pulumiOperations.ConsolidateEnvironment(application.Model, environment.Model, @event.DeploymentId);

                // Todo: Set to actual execution context - might not be the right place for this!
                _executionContextManager.Set(_executionContext);
                await _eventLog.Append(context.EventSourceId, new ApplicationEnvironmentDeploymentCompleted(@event.ApplicationId, @event.EnvironmentId, @event.DeploymentId));
            }
            catch (Exception ex)
            {
                // Todo: Set to actual execution context - might not be the right place for this!
                _executionContextManager.Set(_executionContext);
                var messages = ex.GetAllMessages();
                await _eventLog.Append(context.EventSourceId, new ApplicationEnvironmentDeploymentFailed(@event.ApplicationId, @event.EnvironmentId, @event.DeploymentId, messages, ex.StackTrace ?? string.Empty));
            }
        });

        return Task.CompletedTask;
    }

    public async Task AppSettingsChanged(AppSettingsSetForMicroservice @event, EventContext context)
    {
        var application = (await _projections.GetInstanceById<Application>(@event.ApplicationId)).Model;
        var environment = (await _projections.GetInstanceById<ApplicationEnvironmentWithArtifacts>(@event.EnvironmentId)).Model;
        var microservice = environment.GetMicroserviceById(@event.MicroserviceId);

        var subscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == environment.AzureSubscriptionId);
        var storage = await application.GetStorage(environment, _settings.ServicePrincipal, subscription);
        var fileStorage = new FileStorage(storage.AccountName, storage.AccountKey, microservice.GetFileShareName(), _fileStorageLogger);
        var microserviceStorage = new MicroserviceStorage(application, microservice, fileStorage);
        microserviceStorage.CreateAndUploadAppSettings();
    }

    public Task DeployableImageChanged(DeployableImageChangedInEnvironment @event, EventContext context)
    {
        _ = Task.Run(async () =>
        {
            var application = await _projections.GetInstanceById<Application>(@event.ApplicationId);
            var environment = await _projections.GetInstanceById<ApplicationEnvironmentWithArtifacts>(@event.EnvironmentId);
            var microservice = environment.Model.GetMicroserviceById(@event.MicroserviceId);
            var module = microservice.GetModuleById(@event.ModuleId);
            var deployable = module.GetDeployableById(@event.DeployableId);

            _logger.ChangingDeployableImage(
                microservice.Name,
                deployable.Name,
                @event.Image,
                environment.Model.Name,
                application.Model.Name);

            try
            {
                await _pulumiOperations.SetImageForDeployable(
                    application.Model,
                    environment.Model,
                    microservice,
                    module,
                    @event.DeploymentId,
                    deployable,
                    @event.Image);

                _executionContextManager.Set(_executionContext);
                await _eventLog.Append(context.EventSourceId, new ApplicationEnvironmentDeploymentCompleted(@event.ApplicationId, @event.EnvironmentId, @event.DeploymentId));
            }
            catch (Exception ex)
            {
                // Todo: Set to actual execution context - might not be the right place for this!
                _executionContextManager.Set(_executionContext);
                var messages = ex.GetAllMessages();
                await _eventLog.Append(context.EventSourceId, new ApplicationEnvironmentDeploymentFailed(@event.ApplicationId, @event.EnvironmentId, @event.DeploymentId, messages, ex.StackTrace ?? string.Empty));
            }
        });

        return Task.CompletedTask;
    }
}
