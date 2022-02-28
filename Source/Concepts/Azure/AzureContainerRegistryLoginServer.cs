// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Azure;

public record AzureContainerRegistryLoginServer(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator AzureContainerRegistryLoginServer(string value) => new(value);
}
