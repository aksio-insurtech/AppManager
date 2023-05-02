// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments.Microservices.Modules.Deployables;

namespace Read.Applications;

public record DeployablesProjection : IProjectionFor<Deployable>
{
    public ProjectionId Identifier => "9e1ba755-22b2-4817-b32f-6a8fc19b9a80";

    public void Define(IProjectionBuilderFor<Deployable> builder) => builder
        .From<DeployableCreated>(_ => _
            .UsingCompositeKey<DeployableKey>(key => key
                .Set(k => k.EnvironmentId).ToEventSourceId()
                .Set(k => k.MicroserviceId).To(e => e.MicroserviceId)
                .Set(k => k.DeployableId).To(e => e.DeployableId))
            .Set(m => m.Name).To(e => e.Name))
        .From<DeployableImageChangedInEnvironment>(_ => _
            .UsingCompositeKey<DeployableKey>(key => key
                .Set(k => k.EnvironmentId).ToEventSourceId()
                .Set(k => k.MicroserviceId).To(e => e.MicroserviceId)
                .Set(k => k.DeployableId).To(e => e.DeployableId))
            .Set(m => m.Image).To(e => e.Image));
}
