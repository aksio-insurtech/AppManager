// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Security;

public record Group(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator Group(string group) => new(group);
    public static implicit operator string(Group group) => group.Value;
}
