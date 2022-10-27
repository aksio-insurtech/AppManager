// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications;

public record ApplicationId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static readonly ApplicationId NotSet = Guid.Empty;
    public static implicit operator ApplicationId(Guid value) => new(value);
    public static implicit operator ApplicationId(string value) => new(Guid.Parse(value));
    public static implicit operator EventSourceId(ApplicationId applicationId) => new(applicationId.Value.ToString());
    public static implicit operator ModelKey(ApplicationId applicationId) => new(applicationId.Value.ToString());
}
