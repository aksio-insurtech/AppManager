// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Security;

public record Role(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator Role(string role) => new(role);
    public static implicit operator string(Role role) => role.Value;
}
