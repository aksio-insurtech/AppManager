// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications;

public record ConfigFileContent(string Name) : ConceptAs<string>(Name)
{
    public static implicit operator ConfigFileContent(string name) => new(name);
}
