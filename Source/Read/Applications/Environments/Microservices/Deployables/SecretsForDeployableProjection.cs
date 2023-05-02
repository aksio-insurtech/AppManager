// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments.Microservices.Modules.Deployables;

namespace Read.Applications.Environments.Microservices.Deployables;

public class SecretsForDeployableProjection : IProjectionFor<SecretsForDeployable>
{
    public ProjectionId Identifier => "f414b752-7a81-4f2a-ab42-1f6d39f8d85b";

    public void Define(IProjectionBuilderFor<SecretsForDeployable> builder) => builder
        .Children(m => m.Secrets, _ => _
            .IdentifiedBy(m => m.Key)
            .From<SecretSetForDeployable>(_ => _
                .UsingParentCompositeKey<DeployableOnEnvironmentKey>(key => key
                    .Set(k => k.DeployableId).To(e => e.DeployableId)
                    .Set(k => k.EnvironmentId).To(e => e.EnvironmentId))
                .UsingKey(e => e.Key)
                .Set(m => m.Value).To(e => e.Value)));
}
