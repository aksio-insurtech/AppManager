// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.Network;

namespace Reactions.Applications.Pulumi;

public record NetworkResult(
    VirtualNetwork VirtualNetwork,
    NetworkProfile Profile,
    PrivateZone DnsZone,
    PublicIPAddress IpAddress,
    ApplicationGateway ApplicationGateway);
