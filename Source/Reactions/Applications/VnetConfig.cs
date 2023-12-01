// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions.Applications;

/// <summary>
/// Optional vnet configuration.
/// </summary>
/// <param name="AddressSpace">VNET address-space, defaults to 10.100.0.0/16.</param>
/// <param name="InfraSubnet">Infrastructure subnet, defaults to 10.100.0.0/23.</param>
/// <param name="MongoDbSubnet">MongoDb subnet, defaults to 10.100.2.0/24.</param>
public record VnetConfig(string AddressSpace = "10.100.0.0/16", string InfraSubnet = "10.100.0.0/23", string MongoDbSubnet = "10.100.2.0/24");