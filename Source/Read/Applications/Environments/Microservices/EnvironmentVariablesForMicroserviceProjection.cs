// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments.Microservices;

namespace Read.Applications.Environments.Microservices;

public class EnvironmentVariablesForMicroserviceProjection : IProjectionFor<EnvironmentVariablesForMicroservice>
{
    public ProjectionId Identifier => "ce226005-0704-4d98-8141-03ba1ebf1715";

    public void Define(IProjectionBuilderFor<EnvironmentVariablesForMicroservice> builder) => builder
        .Children(m => m.Variables, _ => _
            .IdentifiedBy(m => m.Key)
            .From<EnvironmentVariableSetForMicroservice>(_ => _
                .UsingParentCompositeKey<MicroserviceOnEnvironmentKey>(key => key
                    .Set(k => k.MicroserviceId).To(e => e.MicroserviceId)
                    .Set(k => k.EnvironmentId).To(e => e.EnvironmentId))
                .UsingKey(e => e.Key)
                .Set(m => m.Value).To(e => e.Value)));
}
