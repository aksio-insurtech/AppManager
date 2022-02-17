// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;
using Microsoft.Extensions.Logging;

namespace Reactions.Applications
{
    [Observer("c2f0e081-6a1a-46e0-bc8c-8a08e0c4dff5")]
    public class ApplicationResourcesCoordinator
    {
        readonly ILogger<ApplicationResources> _logger;
        readonly IPassiveProjectionRepositoryFor<Application> _application;
        readonly IPulumiStackDefinitions _stackDefinitions;
        readonly IPulumiRunner _pulumiRunner;

        public ApplicationResourcesCoordinator(
            ILogger<ApplicationResources> logger,
            IPassiveProjectionRepositoryFor<Application> application,
            IPulumiStackDefinitions stackDefinitions,
            IPulumiRunner pulumiRunner)
        {
            _logger = logger;
            _application = application;
            _stackDefinitions = stackDefinitions;
            _pulumiRunner = pulumiRunner;
        }

        public async Task Created(ApplicationCreated @event, EventContext context)
        {
            var application = await _application.GetById(context.EventSourceId);
            Console.WriteLine(application);
            Console.WriteLine(_logger);
            Console.WriteLine(_stackDefinitions);
            Console.WriteLine(_pulumiRunner);

            // var definition = _stackDefinitions.CreateApplication(application, RuntimeEnvironment.Development);
            // await _pulumiRunner.Up(application, application.Name, definition, RuntimeEnvironment.Development);
            await Task.CompletedTask;
        }

        public async Task Removed(ApplicationRemoved @event, EventContext context)
        {
            // var application = await GetApplication(context.EventSourceId);
            // var definition = _stackDefinitions.CreateApplication(application, RuntimeEnvironment.Development);
            // await _pulumiRunner.Down(application, application.Name, definition, RuntimeEnvironment.Development);
            await Task.CompletedTask;
        }
    }
}
