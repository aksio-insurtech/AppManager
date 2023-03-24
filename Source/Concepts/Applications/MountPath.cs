// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications;

public record MountPath(string Value) : ConceptAs<string>(Value)
{
    public static readonly MountPath Default = new("/app/config");
    public static implicit operator MountPath(string value) => new(value);
}
