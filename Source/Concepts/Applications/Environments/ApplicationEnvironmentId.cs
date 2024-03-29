// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications.Environments;

public record ApplicationEnvironmentId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator EventSourceId(ApplicationEnvironmentId id) => id.Value;

    public static implicit operator ApplicationEnvironmentId(Guid id) => new(id);

    public static implicit operator ApplicationEnvironmentId(EventSourceId id) => Guid.Parse(id.Value);

    public static implicit operator ModelKey(ApplicationEnvironmentId environmentId) => new(environmentId.Value.ToString());

    public static ApplicationEnvironmentId New() => Guid.NewGuid();
}
