// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Infrastructure;
using Pulumi.Automation;

namespace Bootstrap;

public class BootstrapStacksForMicroservices : IStacksForMicroservices
{
    record StackForSaving(ApplicationId ApplicationId, MicroserviceId microserviceId, ApplicationEnvironment Environment, string Deployment);
    #pragma warning disable CS0649
    internal AppManagerApi? AppManagerApi;
    readonly ApplicationId _applicationId;
    readonly List<StackForSaving> _stacksForSaving = new();

    public BootstrapStacksForMicroservices(ApplicationId applicationId)
    {
        _applicationId = applicationId;
    }

    public Task<StackDeployment> GetFor(ApplicationId applicationId, MicroserviceId microserviceId, ApplicationEnvironment environment) => Task.FromResult(StackDeployment.FromJsonString("{}"));

    public Task<bool> HasFor(ApplicationId applicationId, MicroserviceId microserviceId, ApplicationEnvironment environment) => Task.FromResult(false);

    public Task Save(ApplicationId applicationId, MicroserviceId microserviceId, ApplicationEnvironment environment, StackDeployment stackDeployment)
    {
        _stacksForSaving.Add(new(_applicationId, microserviceId, environment, stackDeployment.Json.ToString()));
        return Task.CompletedTask;
    }

    public async Task SaveAllQueued()
    {
        foreach (var stack in _stacksForSaving)
        {
            await (AppManagerApi?.SetStackForMicroservice(stack.ApplicationId, stack.microserviceId, stack.Environment, stack.Deployment) ?? Task.CompletedTask);
        }
    }
}
