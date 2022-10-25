// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Concepts.Applications.Environments;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public interface IPulumiStackDefinitions
{
    Task<ApplicationResult> ApplicationEnvironment(ExecutionContext executionContext, Application application, ApplicationEnvironment environment, SemanticVersion cratisVersion);
    Task Ingress(ExecutionContext executionContext, Application application, ApplicationEnvironment environment, Ingress ingress);
    Task<ContainerAppResult> Microservice(ExecutionContext executionContext, Application application, Microservice microservice, ApplicationEnvironment environment, bool useContainerRegistry = true, ResourceGroup? resourceGroup = default, IEnumerable<Deployable>? deployables = default);
    Task Deployable(ExecutionContext executionContext, Application application, Microservice microservice, IEnumerable<Deployable> deployables, ApplicationEnvironment environment);
}
