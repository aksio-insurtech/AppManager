// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Resources;
using Pulumi.AzureNative.Network;

namespace Reactions.Applications.Pulumi.Resources;

public record RenderContextForResource(
    ResourceId Id,
    ResourceName Name,
    Application Application,
    ApplicationEnvironmentWithArtifacts Environment,
    Storage Storage,
    VirtualNetwork VirtualNetwork);
