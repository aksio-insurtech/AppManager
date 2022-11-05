// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments.Microservices.Deployables;

namespace Read.Applications.Environments.Microservices.Deployables;

public class EnvironmentVariablesForDeployableProjection : IProjectionFor<EnvironmentVariablesForDeployable>
{
    public ProjectionId Identifier => "f4ded7df-b66d-4b22-8d39-426b7f7f81b9";

    public void Define(IProjectionBuilderFor<EnvironmentVariablesForDeployable> builder) => builder
        .Children(m => m.Variables, _ => _
            .IdentifiedBy(m => m.Key)
            .From<EnvironmentVariableSetForDeployable>(_ => _
                .UsingParentKey(e => e.DeployableId)
                .UsingKey(e => e.Key)
                .Set(m => m.Value).To(e => e.Value)));
}
