// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments.Microservices;

namespace Read.Applications.Environments.Microservices;

public class AppSettingsForMicroserviceProjection : IProjectionFor<AppSettingsForMicroservice>
{
    public ProjectionId Identifier => "8d1711f2-1827-42b6-a8ce-e970700c53fe";

    public void Define(IProjectionBuilderFor<AppSettingsForMicroservice> builder) => builder
        .From<AppSettingsSetForMicroservice>(_ => _
            .UsingCompositeKey<MicroserviceOnEnvironmentKey>(key => key
                .Set(k => k.MicroserviceId).To(e => e.MicroserviceId)
                .Set(k => k.EnvironmentId).To(e => e.EnvironmentId))
            .Set(m => m.Content).To(e => e.Content));
}
