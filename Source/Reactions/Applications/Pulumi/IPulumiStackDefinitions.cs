// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public interface IPulumiStackDefinitions
{
    Task<ApplicationEnvironmentResult> ApplicationEnvironment(ExecutionContext executionContext, Application application, ApplicationEnvironmentWithArtifacts environment, SemanticVersion cratisVersion);
    Task<IngressResult> Ingress(ExecutionContext executionContext, Application application, ApplicationEnvironmentWithArtifacts environment, Ingress ingress, ResourceGroup? resourceGroup = default);
    Task<ContainerAppResult> Microservice(ExecutionContext executionContext, Application application, Microservice microservice, ApplicationEnvironmentWithArtifacts environment, bool useContainerRegistry = true, ResourceGroup? resourceGroup = default, IEnumerable<Deployable>? deployables = default);
    Task Deployable(ExecutionContext executionContext, Application application, Microservice microservice, IEnumerable<Deployable> deployables, ApplicationEnvironmentWithArtifacts environment);
}
