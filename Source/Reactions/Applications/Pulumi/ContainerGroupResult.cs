// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.ContainerInstance;

namespace Reactions.Applications.Pulumi;

public record ContainerGroupResult(ContainerGroup ContainerGroup, string IpAddress);
