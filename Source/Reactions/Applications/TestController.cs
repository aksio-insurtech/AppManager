// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;

namespace Reactions.Applications
{
    [Route("/api/test")]
    public class TestController : Controller
    {
        readonly IPulumiStackDefinitions _stackDefinitions;
        readonly IPulumiOperations _pulumiOperations;
        readonly IPassiveProjectionRepositoryFor<Application> _application;
        readonly IPassiveProjectionRepositoryFor<Microservice> _microservice;

        public TestController(
            IPulumiStackDefinitions stackDefinitions,
            IPulumiOperations runner,
            IPassiveProjectionRepositoryFor<Application> application,
            IPassiveProjectionRepositoryFor<Microservice> microservice)
        {
            _stackDefinitions = stackDefinitions;
            _pulumiOperations = runner;
            _application = application;
            _microservice = microservice;
        }

        [HttpGet]
        public async Task Up()
        {
            var application = await _application.GetById("318b19e4-5d7f-4cbc-a7bb-d2a2bf6ede88");
            var definition = _stackDefinitions.Application(application, CloudRuntimeEnvironment.Development);
            _pulumiOperations.Up(application, application.Name, definition, CloudRuntimeEnvironment.Development);
        }

        [HttpGet("down")]
        public async Task Down()
        {
            var application = await _application.GetById("318b19e4-5d7f-4cbc-a7bb-d2a2bf6ede88");
            var definition = _stackDefinitions.Application(application, CloudRuntimeEnvironment.Development);
            _pulumiOperations.Down(application, application.Name, definition, CloudRuntimeEnvironment.Development);
        }

        [HttpGet("microservice")]
        public async Task MicroserviceUp()
        {
            var application = await _application.GetById("318b19e4-5d7f-4cbc-a7bb-d2a2bf6ede88");
            var microservice = await _microservice.GetById("1bbf301b-94d4-47d2-8775-c6e0f0e6bf44");
            var projectName = $"{application.Name}-{microservice.Name}";
            var definition = _stackDefinitions.Microservice(application, microservice, CloudRuntimeEnvironment.Development);
            _pulumiOperations.Up(application, projectName, definition, CloudRuntimeEnvironment.Development);
        }
    }
}
