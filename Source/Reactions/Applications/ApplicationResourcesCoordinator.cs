// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Events.Applications;
using Microsoft.Extensions.Logging;

namespace Reactions.Applications
{
    [Observer("c2f0e081-6a1a-46e0-bc8c-8a08e0c4dff5")]
    public class ApplicationResourcesCoordinator
    {
        readonly ILogger<ApplicationResources> _logger;
        readonly IPassiveProjectionRepositoryFor<Application> _application;
        readonly IPassiveProjectionRepositoryFor<Microservice> _microservice;
        readonly IPulumiStackDefinitions _stackDefinitions;
        readonly IPulumiOperations _pulumiOperations;

        public ApplicationResourcesCoordinator(
            ILogger<ApplicationResources> logger,
            IPassiveProjectionRepositoryFor<Application> application,
            IPassiveProjectionRepositoryFor<Microservice> microservice,
            IPulumiStackDefinitions stackDefinitions,
            IPulumiOperations pulumiOperations)
        {
            _logger = logger;
            _application = application;
            _microservice = microservice;
            _stackDefinitions = stackDefinitions;
            _pulumiOperations = pulumiOperations;
        }

        public async Task ApplicationCreated(ApplicationCreated @event, EventContext context)
        {
            _logger.CreatingApplication(@event.Name);
            var application = await _application.GetById(context.EventSourceId);
            var definition = _stackDefinitions.Application(application, CloudRuntimeEnvironment.Development);
            _pulumiOperations.Up(application, application.Name, definition, CloudRuntimeEnvironment.Development);
            await Task.CompletedTask;
        }

        public async Task Removed(ApplicationRemoved @event, EventContext context)
        {
            var application = await _application.GetById(context.EventSourceId);
            _logger.RemovingApplication(application.Name);
            var definition = _stackDefinitions.Application(application, CloudRuntimeEnvironment.Development);
            _pulumiOperations.Down(application, application.Name, definition, CloudRuntimeEnvironment.Development);
            await Task.CompletedTask;
        }

        public async Task MicroserviceCreated(MicroserviceCreated @event, EventContext context)
        {
            _logger.CreatingMicroservice(@event.Name);
            var application = await _application.GetById(@event.ApplicationId);
            var microservice = await _microservice.GetById(@context.EventSourceId);
            var projectName = GetProjectNameFor(application, microservice);
            var definition = _stackDefinitions.Microservice(application, microservice, CloudRuntimeEnvironment.Development);
            _pulumiOperations.Up(application, projectName, definition, CloudRuntimeEnvironment.Development);
            await _pulumiOperations.SetTag(projectName, CloudRuntimeEnvironment.Development, "microservice", @event.Name);
        }

        public async Task MicroserviceRemoved(MicroserviceRemoved @event, EventContext context)
        {
            var microservice = await _microservice.GetById(@context.EventSourceId);
            _logger.RemovingMicroservice(microservice.Name);
            var application = await _application.GetById(microservice.ApplicationId);
            var projectName = GetProjectNameFor(application, microservice);
            var definition = _stackDefinitions.Microservice(application, microservice, CloudRuntimeEnvironment.Development);
            _pulumiOperations.Down(application, projectName, definition, CloudRuntimeEnvironment.Development);
        }

        string GetProjectNameFor(Application application, Microservice microservice) => $"{application.Name}-{microservice.Name}";
    }
}
