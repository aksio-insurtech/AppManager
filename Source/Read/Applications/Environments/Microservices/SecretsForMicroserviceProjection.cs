// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments.Microservices;

namespace Read.Applications.Environments.Microservices;

public class SecretsForMicroserviceProjection : IProjectionFor<SecretsForMicroservice>
{
    public ProjectionId Identifier => "93dfffa4-ceea-40d8-bdb1-cce23e2f61a5";

    public void Define(IProjectionBuilderFor<SecretsForMicroservice> builder) => builder
        .Children(m => m.Secrets, _ => _
            .IdentifiedBy(m => m.Key)
            .From<SecretSetForMicroservice>(_ => _
                .UsingKey(e => e.Key)
                .Set(m => m.Value).To(e => e.Value)));
}
