// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications;

public record ConfigPath(string Value) : ConceptAs<string>(Value)
{
    public static readonly ConfigPath Default = new("/app/config");
    public static implicit operator ConfigPath(string value) => new(value);
}
