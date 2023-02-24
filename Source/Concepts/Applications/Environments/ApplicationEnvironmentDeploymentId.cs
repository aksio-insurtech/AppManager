// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications.Environments;

public record ApplicationEnvironmentDeploymentId(Guid Value) : ConceptAs<Guid>(Value)
{
    public static implicit operator ApplicationEnvironmentDeploymentId(Guid id) => new(id);

    public static ApplicationEnvironmentDeploymentId New() => Guid.NewGuid();
}
