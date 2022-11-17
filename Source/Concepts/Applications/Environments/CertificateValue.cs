// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications.Environments;

public record CertificateValue(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator CertificateValue(string value) => new(value);
}
