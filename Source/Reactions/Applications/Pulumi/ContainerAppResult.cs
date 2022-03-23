// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.Web.V20210301;

namespace Reactions.Applications.Pulumi;

public record ContainerAppResult(ContainerApp ContainerGroup, string IpAddress);
