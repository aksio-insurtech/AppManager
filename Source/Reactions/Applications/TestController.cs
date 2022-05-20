// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;

namespace Reactions.Applications.Pulumi;

[Route("/api/test")]
public class TestController : Controller
{
    readonly ExecutionContext _executionContext;
    readonly IPulumiStackDefinitions _stackDefinitions;
    readonly IPulumiOperations _pulumiOperations;
    readonly IPassiveProjectionRepositoryFor<Application> _application;
    readonly IPassiveProjectionRepositoryFor<Microservice> _microservice;

    public TestController(
        ExecutionContext executionContext,
        IPulumiStackDefinitions stackDefinitions,
        IPulumiOperations runner,
        IPassiveProjectionRepositoryFor<Application> application,
        IPassiveProjectionRepositoryFor<Microservice> microservice)
    {
        _executionContext = executionContext;
        _stackDefinitions = stackDefinitions;
        _pulumiOperations = runner;
        _application = application;
        _microservice = microservice;
    }

    [HttpGet]
    public async Task Up()
    {
        var application = await _application.GetById("318b19e4-5d7f-4cbc-a7bb-d2a2bf6ede88");
        var definition = _stackDefinitions.Application(_executionContext, application, CloudRuntimeEnvironment.Development);
        _pulumiOperations.Up(application, application.Name, definition, CloudRuntimeEnvironment.Development);
    }

    [HttpGet("containerapp")]
    public async Task UpContainerApp()
    {
        var application = await _application.GetById("cd9b5601-1966-4c67-a393-039386dfba20");
        var definition = _stackDefinitions.Application(_executionContext, application, CloudRuntimeEnvironment.Development);
        _pulumiOperations.Up(application, application.Name, definition, CloudRuntimeEnvironment.Development);
    }

    [HttpGet("down")]
    public async Task Down()
    {
        var application = await _application.GetById("318b19e4-5d7f-4cbc-a7bb-d2a2bf6ede88");
        var definition = _stackDefinitions.Application(_executionContext, application, CloudRuntimeEnvironment.Development);
        _pulumiOperations.Down(application, application.Name, definition, CloudRuntimeEnvironment.Development);
    }

    [HttpGet("microservice")]
    public async Task MicroserviceUp()
    {
        var application = await _application.GetById("318b19e4-5d7f-4cbc-a7bb-d2a2bf6ede88");
        var microservice = await _microservice.GetById("1bbf301b-94d4-47d2-8775-c6e0f0e6bf44");
        var projectName = $"{application.Name}-{microservice.Name}";
        var definition = _stackDefinitions.Microservice(_executionContext, application, microservice, CloudRuntimeEnvironment.Development);
        _pulumiOperations.Up(application, projectName, definition, CloudRuntimeEnvironment.Development);
    }

    [HttpGet("microservice/containerapp")]
    public async Task MicroserviceUpContainerApp()
    {
        var application = await _application.GetById("cd9b5601-1966-4c67-a393-039386dfba20");
        var microservice = await _microservice.GetById("b2c57f22-4e45-41e8-9593-c5a3f8f8b9ca");
        var projectName = $"{application.Name}-{microservice.Name}";
        var definition = _stackDefinitions.Microservice(_executionContext, application, microservice, CloudRuntimeEnvironment.Development);
        _pulumiOperations.Up(application, projectName, definition, CloudRuntimeEnvironment.Development);
    }

    [HttpGet("microservice/down")]
    public async Task MicroserviceDown()
    {
        var application = await _application.GetById("318b19e4-5d7f-4cbc-a7bb-d2a2bf6ede88");
        var microservice = await _microservice.GetById("1bbf301b-94d4-47d2-8775-c6e0f0e6bf44");
        var projectName = $"{application.Name}-{microservice.Name}";
        var definition = _stackDefinitions.Microservice(_executionContext, application, microservice, CloudRuntimeEnvironment.Development);
        _pulumiOperations.Down(application, projectName, definition, CloudRuntimeEnvironment.Development);
    }

    [HttpGet("deployable")]
    public async Task DeployableUp()
    {
        var application = await _application.GetById("318b19e4-5d7f-4cbc-a7bb-d2a2bf6ede88");
        var microservice = await _microservice.GetById("1bbf301b-94d4-47d2-8775-c6e0f0e6bf44");
        var projectName = $"{application.Name}-{microservice.Name}";
        var deployable = new Deployable(Guid.Parse("684196c5-a785-42cd-b737-9fbf2fe326b6"), new(Guid.Parse("1bbf301b-94d4-47d2-8775-c6e0f0e6bf44")), "main", "nginx", new[] { 80 });
        var definition = _stackDefinitions.Deployable(_executionContext, application, microservice, deployable, CloudRuntimeEnvironment.Development);
        _pulumiOperations.Up(application, projectName, definition, CloudRuntimeEnvironment.Development);
    }
}
