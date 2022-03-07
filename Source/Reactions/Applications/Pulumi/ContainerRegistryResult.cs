// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.ContainerRegistry;

namespace Reactions.Applications.Pulumi;

public record ContainerRegistryResult(Registry Registry, string LoginServer, string UserName, string Password);
