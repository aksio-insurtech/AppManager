// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi;

namespace Reactions.Applications.Pulumi;

public static class VirtualNetworkExtensions
{
    /// <summary>
    /// Gets the id for the subnet named 'infrastructure' in this NetworkResult's VirtualNetwork.
    /// </summary>
    /// <param name="vnet">The network to use.</param>
    /// <returns>Thet network id.</returns>
    public static Input<string> GetInfrastructureSubnetId(this NetworkResult vnet)
    {
        return vnet.VirtualNetwork.Subnets.Apply(_ => _.First(s => s.Name == "infrastructure").Id!);
    }
}