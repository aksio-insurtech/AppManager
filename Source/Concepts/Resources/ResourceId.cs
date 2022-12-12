// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Resources;

public record ResourceId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator ResourceId(string value) => new(Guid.Parse(value));
}
