// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications;

public record DeployableId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator EventSourceId(DeployableId deployableId) => new(deployableId.Value.ToString());
    public static implicit operator DeployableId(Guid value) => new(value);
    public static implicit operator DeployableId(EventSourceId value) => Guid.Parse(value.Value);
    public static implicit operator ModelKey(DeployableId microserviceId) => new(microserviceId.Value.ToString());
}
