// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications.Ingresses;

public record IngressId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator EventSourceId(IngressId id) => id.Value;
}
