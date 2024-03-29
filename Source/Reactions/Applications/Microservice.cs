// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Reactions.Applications;

public record Microservice(
    MicroserviceId Id,
    MicroserviceName Name,
    AppSettingsContent AppSettingsContent,
    IEnumerable<Deployable> Deployables,
    IEnumerable<MicroserviceId> ConnectedWith,
    int Port = 80)
{
    public Deployable GetDeployableById(DeployableId id) => Deployables.Single(_ => _.Id == id);
}
