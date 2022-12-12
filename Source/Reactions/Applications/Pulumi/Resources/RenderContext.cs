// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.Network;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi.Resources;

public record RenderContext(
    Application Application,
    ApplicationEnvironmentWithArtifacts Environment,
    ResourceGroup ResourceGroup,
    Tags Tags,
    Storage Storage,
    VirtualNetwork VirtualNetwork);
