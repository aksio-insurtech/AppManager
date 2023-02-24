// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Concepts.Applications.Environments;
using Pulumi;
using Pulumi.AzureNative.App;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public interface IPulumiStackDefinitions
{
    Task Application(Application application, ApplicationEnvironmentWithArtifacts sharedEnvironment);

    Task<ApplicationEnvironmentResult> ApplicationEnvironment(
        ExecutionContext executionContext,
        Application application,
        ApplicationEnvironmentWithArtifacts environment,
        ApplicationEnvironmentWithArtifacts sharedEnvironment,
        SemanticVersion cratisVersion);

    Task<IngressResult> Ingress(
        ExecutionContext executionContext,
        Application application,
        ApplicationEnvironmentWithArtifacts environment,
        Ingress ingress,
        ManagedEnvironment managedEnvironment,
        IDictionary<CertificateId, Output<string>> certificates,
        ResourceGroup? resourceGroup = default);

    Task<ContainerAppResult> Microservice(
        ExecutionContext executionContext,
        Application application,
        Microservice microservice,
        ResourceGroup resourceGroup,
        ApplicationEnvironmentWithArtifacts environment,
        ManagedEnvironment managedEnvironment,
        bool useContainerRegistry = true,
        IEnumerable<Deployable>? deployables = default);
}
