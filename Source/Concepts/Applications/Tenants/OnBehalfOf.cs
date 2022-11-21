// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications.Tenants;

public record OnBehalfOf(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator OnBehalfOf(string name) => new(name);
}
