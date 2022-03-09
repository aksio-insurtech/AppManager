// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public record ApplicationResult(
    CloudRuntimeEnvironment Environment,
    ResourceGroup ResourceGroup,
    NetworkResult Network,
    StorageResult Storage,
    ContainerRegistryResult ContainerRegistry,
    MongoDBResult MongoDB,
    ContainerGroupResult Kernel);
