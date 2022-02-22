// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;

namespace Read.Applications.Microservices
{
    public class MicroservicesProjection : IProjectionFor<Microservice>
    {
        public ProjectionId Identifier => "57fe47e1-3485-424f-b892-9da207f6729f";

        public void Define(IProjectionBuilderFor<Microservice> builder) => builder
            .From<MicroserviceCreated>(_ => _
                .Set(m => m.Name).To(e => e.Name));
    }
}
