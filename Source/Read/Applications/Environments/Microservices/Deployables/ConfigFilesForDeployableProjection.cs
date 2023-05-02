// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments.Microservices.Modules.Deployables;

namespace Read.Applications.Environments.Microservices.Deployables;

public class ConfigFilesForDeployableProjection : IProjectionFor<ConfigFilesForDeployable>
{
    public ProjectionId Identifier => "07210321-bfac-4e88-92f2-75c041b187e8";

    public void Define(IProjectionBuilderFor<ConfigFilesForDeployable> builder) => builder
        .Children(m => m.Files, _ => _
            .IdentifiedBy(m => m.Name)
            .From<ConfigFileSetForDeployable>(_ => _
                .UsingParentCompositeKey<DeployableOnEnvironmentKey>(key => key
                    .Set(k => k.DeployableId).To(e => e.DeployableId)
                    .Set(k => k.EnvironmentId).To(e => e.EnvironmentId))
                .UsingKey(e => e.Name)
                .Set(m => m.Content).To(e => e.Content)));
}
