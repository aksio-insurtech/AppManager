// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications.Environments.Ingresses;

public record IngressId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator EventSourceId(IngressId id) => id.Value;
    public static implicit operator IngressId(EventSourceId id) => new(Guid.Parse(id.Value));
    public static implicit operator IngressId(Guid id) => new(id);
}
