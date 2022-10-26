// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Pulumi.AzureNative.App;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public record ApplicationEnvironmentResult(
    ApplicationEnvironment Environment,
    ResourceGroup ResourceGroup,
    NetworkResult Network,
    StorageResult Storage,
    ContainerRegistryResult ContainerRegistry,
    MongoDBResult MongoDB,
    ManagedEnvironment ManagedEnvironment,
    ContainerAppResult Kernel)
{
    public Task<Application> MergeWithApplication(Application application, ApplicationEnvironmentWithArtifacts environment)
    {
        return Task.FromResult<Application>(new(
            application.Id,
            application.Name,
            application.Environments
        ));
    }
}
