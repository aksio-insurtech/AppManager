// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications.Environments.AccessList;

/// <summary>
/// Represents an IP-address used in access lists.
/// </summary>
/// <param name="Value">The IP address.</param>
public record AccessListAddress(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator AccessListAddress(string value) => new(value);
}
