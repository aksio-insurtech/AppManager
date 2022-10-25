// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments;
using Events.Applications.Environments.Ingresses;
using Events.Applications.Environments.Microservices;
using Events.Applications.Environments.Microservices.Deployables;
using Microsoft.Extensions.Logging;
using Pulumi.Automation;
using Reactions.Applications.Pulumi;

namespace Reactions.Applications;

/// <summary>
/// [Observer("c2f0e081-6a1a-46e0-bc8c-8a08e0c4dff5")].
/// </summary>
public class ApplicationResourcesCoordinator
{
    readonly ILogger<ApplicationResources> _logger;
    readonly IImmediateProjections _projections;
    readonly ExecutionContext _executionContext;
    readonly IPulumiStackDefinitions _stackDefinitions;
    readonly IPulumiOperations _pulumiOperations;

    public ApplicationResourcesCoordinator(
        ILogger<ApplicationResources> logger,
        IImmediateProjections projections,
        ExecutionContext executionContext,
        IPulumiStackDefinitions stackDefinitions,
        IPulumiOperations pulumiOperations)
    {
        _logger = logger;
        _projections = projections;
        _executionContext = executionContext;
        _stackDefinitions = stackDefinitions;
        _pulumiOperations = pulumiOperations;
    }

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
        _ = Task.Run(async () =>
        {
            var definition = PulumiFn.Create(() => _stackDefinitions.Ingress(_executionContext, application, environment, ingress));
            await _pulumiOperations.Up(application, application.Name, definition, environment);
        });
    }

    public Task MicroserviceCreated(MicroserviceCreated @event, EventContext context)
    {
        _logger.CreatingMicroservice(@event.Name);

        _ = Task.Run(async () =>
        {
            var application = await _projections.GetInstanceById<Application>(@event.ApplicationId);
            var microservice = await _projections.GetInstanceById<Microservice>(@context.EventSourceId);
            var environment = application.GetEnvironmentById(@context.EventSourceId);
            var projectName = GetProjectNameFor(application, microservice);

            var definition = PulumiFn.Create(() => _stackDefinitions.Microservice(_executionContext, application, microservice, environment));

            await _pulumiOperations.Up(application, projectName, definition, environment, microservice);
            await _pulumiOperations.SetTag(projectName, environment, "Microservice", @event.Name);
        });

        return Task.CompletedTask;
    }

    public Task DeployableCreated(DeployableCreated @event, EventContext context)
    {
        _ = Task.Run(async () =>
        {
            var deployable = await _projections.GetInstanceById<Deployable>(@context.EventSourceId);
            var microservice = await _projections.GetInstanceById<Microservice>(deployable.MicroserviceId);
            var application = await _projections.GetInstanceById<Application>(microservice.ApplicationId);
            var environment = application.GetEnvironmentById(@context.EventSourceId);
            var projectName = GetProjectNameFor(application, microservice);

            if (deployable.Image is null)
            {
                deployable = new(deployable.Id, deployable.MicroserviceId, deployable.Name, "nginx", deployable.Ports);
            }
            _logger.DeployableCreated(microservice.Name, deployable.Name, deployable.Image);

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

        return Task.CompletedTask;
    }

    public Task DeployableImageChanged(DeployableImageChanged @event, EventContext context)
    {
        _ = Task.Run(async () =>
        {
            var deployable = await _projections.GetInstanceById<Deployable>(@context.EventSourceId);
            var microservice = await _projections.GetInstanceById<Microservice>(deployable.MicroserviceId);
            var application = await _projections.GetInstanceById<Application>(microservice.ApplicationId);
            var environment = application.GetEnvironmentById(@context.EventSourceId);
            _logger.ChangingDeployableImage(microservice.Name, deployable.Name, deployable.Image);
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

        return Task.CompletedTask;
    }

    string GetProjectNameFor(Application application, Microservice microservice) => $"{application.Name}-{microservice.Name}";
}
