// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Infrastructure;
using Pulumi.Automation;

namespace Bootstrap;

public class BootstrapStacksForApplications : IStacksForApplications
{
    record StackForSaving(ApplicationId ApplicationId, ApplicationEnvironment Environment, string Deployment);
    #pragma warning disable CS0649
    internal AppManagerApi? AppManagerApi;
    readonly List<StackForSaving> _stacksForSaving = new();

    public Task<StackDeployment> GetFor(ApplicationId applicationId, ApplicationEnvironment environment) => Task.FromResult(StackDeployment.FromJsonString("{}"));

    public Task<bool> HasFor(ApplicationId applicationId, ApplicationEnvironment environment) => Task.FromResult(false);

    public Task Save(ApplicationId applicationId, ApplicationEnvironment environment, StackDeployment stackDeployment)
    {
        _stacksForSaving.Add(new(applicationId, environment, stackDeployment.Json.ToString()));
        return Task.CompletedTask;
    }

    public async Task SaveAllQueued()
    {
        foreach (var stack in _stacksForSaving)
        {
            await (AppManagerApi?.SetStackForApplication(stack.ApplicationId, stack.Environment, stack.Deployment) ?? Task.CompletedTask);
        }
    }
}
