// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Events.Applications;
using Microsoft.Extensions.Logging;
using Pulumi.Automation;
using Reactions.Applications.Pulumi;

namespace Reactions.Applications;

[Observer("c2f0e081-6a1a-46e0-bc8c-8a08e0c4dff5")]
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

    public async Task ApplicationCreated(ApplicationCreated @event, EventContext context)
    {
        _logger.CreatingApplication(@event.Name);
        var application = await _projections.GetInstanceById<Application>(context.EventSourceId);
        var definition = PulumiFn.Create(() => _stackDefinitions.Application(_executionContext, application, CloudRuntimeEnvironment.Development));
        _pulumiOperations.Up(application, application.Name, definition, CloudRuntimeEnvironment.Development);
        await Task.CompletedTask;
    }

    public async Task Removed(ApplicationRemoved @event, EventContext context)
    {
        var application = await _projections.GetInstanceById<Application>(context.EventSourceId);
        _logger.RemovingApplication(application.Name);
        var definition = PulumiFn.Create(() => _stackDefinitions.Application(_executionContext, application, CloudRuntimeEnvironment.Development));
        _pulumiOperations.Down(application, application.Name, definition, CloudRuntimeEnvironment.Development);
        await Task.CompletedTask;
    }

    public async Task MicroserviceCreated(MicroserviceCreated @event, EventContext context)
    {
        _logger.CreatingMicroservice(@event.Name);
        var application = await _projections.GetInstanceById<Application>(@event.ApplicationId);
        var microservice = await _projections.GetInstanceById<Microservice>(@context.EventSourceId);
        var projectName = GetProjectNameFor(application, microservice);

        var definition = PulumiFn.Create(() => _stackDefinitions.Microservice(_executionContext, application, microservice, CloudRuntimeEnvironment.Development));

        _pulumiOperations.Up(application, projectName, definition, CloudRuntimeEnvironment.Development);
        await _pulumiOperations.SetTag(projectName, CloudRuntimeEnvironment.Development, "Microservice", @event.Name);
    }

    public async Task DeployableImageChanged(DeployableImageChanged @event, EventContext context)
    {
        var deployable = await _projections.GetInstanceById<Deployable>(@context.EventSourceId);
        var microservice = await _projections.GetInstanceById<Microservice>(deployable.MicroserviceId);
        var application = await _projections.GetInstanceById<Application>(microservice.ApplicationId);
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
                CloudRuntimeEnvironment.Development));

        _pulumiOperations.Up(application, projectName, definition, CloudRuntimeEnvironment.Development);
    }

    string GetProjectNameFor(Application application, Microservice microservice) => $"{application.Name}-{microservice.Name}";
}
