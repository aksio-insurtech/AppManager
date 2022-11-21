// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications.Environments.Ingresses.IdentityProviders;

public record IdentityProviderName(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator IdentityProviderName(string value) => new(value);
}
