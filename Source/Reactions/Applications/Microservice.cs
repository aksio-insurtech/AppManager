// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Reactions.Applications;

public record Microservice(
    MicroserviceId Id,
    MicroserviceName Name,
    IEnumerable<Deployable> Deployables)
{
    public Deployable GetDeployableById(DeployableId id) => Deployables.Single(_ => _.Id == id);
}
