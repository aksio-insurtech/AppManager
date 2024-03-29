// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications;

public record ApplicationName(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator ApplicationName(string value) => new(value);
}
