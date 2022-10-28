// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications.Environments;

public record CertificateId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator CertificateId(Guid value) => new(value);

    public static implicit operator CertificateId(string value) => new(Guid.Parse(value));
}
