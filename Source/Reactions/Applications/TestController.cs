// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;

namespace Reactions.Applications
{
    [Route("/api/test")]
    public class TestController : Controller
    {
        readonly IPulumiStackDefinitions _stackDefinitions;
        readonly IPulumiRunner _runner;
        readonly IPassiveProjectionRepositoryFor<Application> _application;

        public TestController(IPulumiStackDefinitions stackDefinitions, IPulumiRunner runner, IPassiveProjectionRepositoryFor<Application> application)
        {
            _stackDefinitions = stackDefinitions;
            _runner = runner;
            _application = application;
        }

        [HttpGet]
        public async Task Up()
        {
            var application = await _application.GetById("318b19e4-5d7f-4cbc-a7bb-d2a2bf6ede88");
            var definition = _stackDefinitions.CreateApplication(application, CloudRuntimeEnvironment.Development);
            _runner.Up(application, application.Name, definition, CloudRuntimeEnvironment.Development);
        }

        [HttpGet("down")]
        public async Task Down()
        {
            var application = await _application.GetById("318b19e4-5d7f-4cbc-a7bb-d2a2bf6ede88");
            var definition = _stackDefinitions.CreateApplication(application, CloudRuntimeEnvironment.Development);
            _runner.Down(application, application.Name, definition, CloudRuntimeEnvironment.Development);
        }
    }
}
