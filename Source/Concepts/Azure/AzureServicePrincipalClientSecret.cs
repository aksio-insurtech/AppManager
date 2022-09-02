// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Azure;

public record AzureServicePrincipalClientSecret(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator AzureServicePrincipalClientSecret(string value) => new(value);
}
