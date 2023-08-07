// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using Concepts.Applications.Environments;

namespace Reactions.Applications.Pulumi;

public class DuplicateCertificateIdsUsedInConfiguration : Exception
{
    public IImmutableList<CertificateId> Duplicates { get; }

    public DuplicateCertificateIdsUsedInConfiguration(List<CertificateId> duplicates) : base($"Duplicate certificateId(s) used in configuration: {string.Join(", ", duplicates)}")
    {
        Duplicates = duplicates.ToImmutableList();
    }
}