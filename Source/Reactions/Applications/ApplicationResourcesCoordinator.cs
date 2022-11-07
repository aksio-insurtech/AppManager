// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments;
using Microsoft.Extensions.Logging;
using Reactions.Applications.Pulumi;

namespace Reactions.Applications;

#pragma warning disable IDE0052

[Observer("c2f0e081-6a1a-46e0-bc8c-8a08e0c4dff5")]
public class ApplicationResourcesCoordinator
{
    readonly ILogger<ApplicationResources> _logger;
    readonly IImmediateProjections _projections;
    readonly ExecutionContext _executionContext;
    readonly IPulumiStackDefinitions _stackDefinitions;
    readonly IPulumiOperations _pulumiOperations;
    readonly IEventLog _eventLog;

    public ApplicationResourcesCoordinator(
        ILogger<ApplicationResources> logger,
        IImmediateProjections projections,
        ExecutionContext executionContext,
        IPulumiStackDefinitions stackDefinitions,
        IPulumiOperations pulumiOperations,
        IEventLog eventLog)
    {
        _logger = logger;
        _projections = projections;
        _executionContext = executionContext;
        _stackDefinitions = stackDefinitions;
        _pulumiOperations = pulumiOperations;
        _eventLog = eventLog;
    }

    public async Task ConsolidationStarted(ApplicationEnvironmentConsolidationStarted @event, EventContext context)
    {
        var application = await _projections.GetInstanceById<Application>(@event.ApplicationId);
        var environment = await _projections.GetInstanceById<ApplicationEnvironmentWithArtifacts>(context.EventSourceId);

        Console.WriteLine(application);
        Console.WriteLine(environment);

        // _logger.ConsolidationStarted(environment.Name, application.Name);
        await Task.Delay(5000);
        await _eventLog.Append(context.EventSourceId, new ApplicationEnvironmentConsolidationCompleted(@event.ApplicationId, @event.EnvironmentId, @event.ConsolidationId));
    }

    /*
    public async Task ApplicationEnvironmentCreated(ApplicationEnvironmentCreated @event, EventContext context)
    {
        var application = await _projections.GetInstanceById<Application>(context.EventSourceId);
        var environment = application.GetEnvironmentById(context.EventSourceId);
        _logger.CreatingApplicationEnvironment(@event.Name, application.Name);

        _ = Task.Run(async () =>
        {
            var definition = PulumiFn.Create(() => _stackDefinitions.ApplicationEnvironment(_executionContext, application, environment, @event.CratisVersion));
            await _pulumiOperations.Up(application, application.Name, definition, environment);
        });
    }

    public async Task IngressCreated(IngressCreated @event, EventContext context)
    {
        var application = await _projections.GetInstanceById<Application>(@event.ApplicationId);
        var environment = application.GetEnvironmentById(@event.EnvironmentId);
        var ingress = environment.GetIngressById(context.EventSourceId);
        _logger.CreatingIngress(@event.Name, environment.Name, application.Name);
        _ = Task.Run(async () =>
        {
            var definition = PulumiFn.Create(() => _stackDefinitions.Ingress(_executionContext, application, environment, ingress));
            await _pulumiOperations.Up(application, application.Name, definition, environment);
        });
    }

    public async Task MicroserviceCreated(MicroserviceCreated @event, EventContext context)
    {
        var application = await _projections.GetInstanceById<Application>(@event.ApplicationId);
        var environment = application.GetEnvironmentById(@event.EnvironmentId);
        var microservice = environment.GetMicroserviceById(context.EventSourceId);
        _logger.CreatingMicroservice(@event.Name, environment.Name, application.Name);

        _ = Task.Run(async () =>
        {
            var projectName = GetProjectNameFor(application, microservice);

            var definition = PulumiFn.Create(() => _stackDefinitions.Microservice(_executionContext, application, microservice, environment));

            await _pulumiOperations.Up(application, projectName, definition, environment, microservice);
            await _pulumiOperations.SetTag(projectName, environment, "Microservice", @event.Name);
        });
    }

    public async Task DeployableCreated(DeployableCreated @event, EventContext context)
    {
        var application = await _projections.GetInstanceById<Application>(@event.ApplicationId);
        var environment = application.GetEnvironmentById(@event.EnvironmentId);
        var microservice = environment.GetMicroserviceById(@event.MicroserviceId);
        var deployable = microservice.GetDeployableById(@context.EventSourceId);
        _logger.DeployableCreated(microservice.Name, environment.Name, application.Name, deployable.Name, deployable.Image);

        _ = Task.Run(async () =>
        {
            var projectName = GetProjectNameFor(application, microservice);

            if (string.IsNullOrEmpty(deployable.Image))
            {
                deployable = new(deployable.Id, deployable.MicroserviceId, deployable.Name, "nginx", deployable.Ports);
            }

            var definition = PulumiFn.Create(() =>
                _stackDefinitions.Deployable(
                    _executionContext,
                    application,
                    microservice,
                    new[]
                    {
                        deployable
                    },
                    environment));

            await _pulumiOperations.Up(application, projectName, definition, environment, microservice);
        });
    }

    public async Task DeployableImageChanged(DeployableImageChanged @event, EventContext context)
    {
        var application = await _projections.GetInstanceById<Application>(@event.ApplicationId);
        var environment = application.GetEnvironmentById(@event.EnvironmentId);
        var microservice = environment.GetMicroserviceById(@event.MicroserviceId);
        var deployable = microservice.GetDeployableById(@context.EventSourceId);
        _logger.ChangingDeployableImage(microservice.Name, deployable.Name, deployable.Image, environment.Name, application.Name);

        _ = Task.Run(async () =>
        {
            var projectName = GetProjectNameFor(application, microservice);

            var definition = PulumiFn.Create(() =>
                _stackDefinitions.Deployable(
                    _executionContext,
                    application,
                    microservice,
                    new[]
                    {
                        deployable
                    },
                    environment));

            await _pulumiOperations.Up(application, projectName, definition, environment, microservice);
        });
    }

    public async Task ConsolidateAllForEnvironment(ApplicationId applicationId, ApplicationEnvironmentId environmentId)
    {
        var application = await _projections.GetInstanceById<Application>(applicationId);
        var environment = application.GetEnvironmentById(environmentId);

        // var projectName = GetProjectNameFor(application, microservice);
        // var definition = PulumiFn.Create(() => _stackDefinitions.ApplicationEnvironment(_executionContext, application, environment, @event.CratisVersion));
        // await _pulumiOperations.Up(application, application.Name, definition, environment);
        foreach (var ingress in environment.Ingresses)
        {
            Console.WriteLine(ingress);
        }

        foreach (var microservice in environment.Microservices)
        {
            Console.WriteLine(microservice);

            foreach (var deployable in microservice.Deployables)
            {
                Console.WriteLine(deployable);
            }
        }
    }

    string GetProjectNameFor(Application application, Microservice microservice) => $"{application.Name}-{microservice.Name}";
    */
}
