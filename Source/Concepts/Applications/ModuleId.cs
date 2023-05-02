// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications;

public record ModuleId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator EventSourceId(ModuleId moduleId) => new(moduleId.Value.ToString());
    public static implicit operator ModuleId(Guid value) => new(value);
    public static implicit operator ModuleId(EventSourceId value) => Guid.Parse(value.Value);
    public static implicit operator ModuleId(string value) => Guid.Parse(value);
    public static implicit operator ModelKey(ModuleId moduleId) => new(moduleId.Value.ToString());
}
