// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public interface IPulumiStackDefinitions
{
    Task<ApplicationResult> Application(ExecutionContext executionContext, Application application, CloudRuntimeEnvironment environment, bool ignoreIngress = false);
    Task<ContainerAppResult> Microservice(ExecutionContext executionContext, Application application, Microservice microservice, CloudRuntimeEnvironment environment, bool useContainerRegistry = true, ResourceGroup? resourceGroup = default, IEnumerable<Deployable>? deployables = default);
    Task Deployable(ExecutionContext executionContext, Application application, Microservice microservice, IEnumerable<Deployable> deployables, CloudRuntimeEnvironment environment);
}
