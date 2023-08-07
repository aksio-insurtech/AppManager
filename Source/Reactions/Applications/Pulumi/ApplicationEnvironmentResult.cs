// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Pulumi;
using Pulumi.AzureNative.App.V20221001;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public record ApplicationEnvironmentResult(
    ApplicationEnvironmentWithArtifacts Environment,
    ResourceGroup ResourceGroup,
    NetworkResult Network,
    StorageResult Storage,
    ManagedEnvironment ManagedEnvironment,
    IDictionary<CertificateId, Output<string>> Certificates)
{
    public Task<ApplicationEnvironmentWithArtifacts> MergeWithApplicationEnvironment(ApplicationEnvironmentWithArtifacts environment)
    {
        return Task.FromResult<ApplicationEnvironmentWithArtifacts>(new(
            environment.Id,
            environment.Name,
            environment.DisplayName,
            environment.ShortName,
            environment.AzureSubscriptionId,
            environment.CloudLocation,
            environment.MongoDB,
            environment.Certificates,
            environment.Tenants,
            environment.Ingresses,
            environment.Microservices,
            environment.BackupEnabled,
            environment.BackupCopyRegion,
            environment.Resources,
            environment.Storage));
    }
}
