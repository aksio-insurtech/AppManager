// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications;

public record SecretValue(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator SecretValue(string value) => new(value);
}
