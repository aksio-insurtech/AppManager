// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments.Microservices.Deployables;

namespace Read.Applications.Environments.Microservices.Deployables;

public class ConfigFilesForDeployableProjection : IProjectionFor<ConfigFilesForDeployable>
{
    public ProjectionId Identifier => "f15edc01-c1a0-4cb8-b2f5-f215507af05a";

    public void Define(IProjectionBuilderFor<ConfigFilesForDeployable> builder) => builder
        .Children(m => m.Files, _ => _
            .IdentifiedBy(m => m.Name)
            .From<ConfigFileSetForDeployable>(_ => _
                .UsingParentKey(e => e.MicroserviceId)
                .UsingKey(e => e.Name)
                .Set(m => m.Content).To(e => e.Content)));
}
