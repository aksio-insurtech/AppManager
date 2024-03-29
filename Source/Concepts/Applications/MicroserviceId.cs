// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications;

public record MicroserviceId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator EventSourceId(MicroserviceId microserviceId) => new(microserviceId.Value.ToString());
    public static implicit operator MicroserviceId(Guid value) => new(value);
    public static implicit operator MicroserviceId(EventSourceId value) => Guid.Parse(value.Value);
    public static implicit operator MicroserviceId(string value) => Guid.Parse(value);
    public static implicit operator ModelKey(MicroserviceId microserviceId) => new(microserviceId.Value.ToString());
}
