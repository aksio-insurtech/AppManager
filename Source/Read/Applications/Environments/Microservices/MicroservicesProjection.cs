// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments.Microservices;

namespace Read.Applications.Environments.Microservices;

public class MicroservicesProjection : IProjectionFor<Microservice>
{
    public ProjectionId Identifier => "57fe47e1-3485-424f-b892-9da207f6729f";

    public void Define(IProjectionBuilderFor<Microservice> builder) => builder
        .From<MicroserviceCreated>(_ => _
            .UsingCompositeKey<MicroserviceKey>(key => key
                .Set(k => k.MicroserviceId).To(e => e.MicroserviceId)
                .Set(k => k.EnvironmentId).ToEventSourceId())
            .Set(m => m.Name).To(e => e.Name))
        .RemovedWith<MicroserviceRemoved>();
}
