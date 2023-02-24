// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments.Microservices;

namespace Read.Applications.Environments.Microservices;

public class ConfigFilesForMicroserviceProjection : IProjectionFor<ConfigFilesForMicroservice>
{
    public ProjectionId Identifier => "f15edc01-c1a0-4cb8-b2f5-f215507af05a";

    public void Define(IProjectionBuilderFor<ConfigFilesForMicroservice> builder) => builder
        .Children(m => m.Files, _ => _
            .IdentifiedBy(m => m.Name)
            .From<ConfigFileSetForMicroservice>(_ => _
                .UsingParentCompositeKey<MicroserviceOnEnvironmentKey>(key => key
                    .Set(k => k.MicroserviceId).To(e => e.MicroserviceId)
                    .Set(k => k.EnvironmentId).To(e => e.EnvironmentId))
                .UsingKey(e => e.Name)
                .Set(m => m.Content).To(e => e.Content)));
}
