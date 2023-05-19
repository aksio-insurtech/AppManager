// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications.Environments.Ingresses;

/// <summary>
/// IP to use as DNS resolver with a ingress.
/// </summary>
/// <param name="Value">The ip address to use.</param>
public record Resolver(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator Resolver(string value) => new(value);
}