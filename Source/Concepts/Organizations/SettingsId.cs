// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Organizations;

public record SettingsId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static readonly SettingsId Global = new(Guid.Empty);

    public static implicit operator EventSourceId(SettingsId settings) => new(settings.Value.ToString());
}
