// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications.Environments;

public record ApplicationEnvironmentConsolidationId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator ApplicationEnvironmentConsolidationId(Guid id) => new(id);

    public static ApplicationEnvironmentConsolidationId New() => Guid.NewGuid();
}
