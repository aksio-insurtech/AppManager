// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Infrastructure;
using Pulumi.Automation;

namespace Bootstrap;

public class BootstrapStacks : IStacks
{
    internal static AppManagerApi? AppManagerApi;

    public Task<StackDeployment> GetFor(ApplicationId applicationId) => Task.FromResult(StackDeployment.FromJsonString("{}"));

    public Task<bool> HasFor(ApplicationId applicationId) => Task.FromResult(false);

    public async Task Save(ApplicationId applicationId, StackDeployment stackDeployment)
    {
        var stackAsJson = stackDeployment.Json.ToString();
        await File.WriteAllTextAsync("/Users/einari/Projects/Aksio/stack.json", stackAsJson);
        if (AppManagerApi != null)
        {
            await AppManagerApi.SetStack(applicationId, stackAsJson);
        }
    }
}