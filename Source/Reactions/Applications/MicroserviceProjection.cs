// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;

namespace Reactions.Applications
{
    public class MicroserviceProjection : IPassiveProjectionFor<Microservice>
    {
        public ProjectionId Identifier => "163f84c8-ce22-4ecc-99da-84516be8e2ab";

        public void Define(IProjectionBuilderFor<Microservice> builder) => builder
            .From<MicroserviceCreated>(_ => _
                .Set(m => m.ApplicationId).To(e => e.ApplicationId)
                .Set(m => m.Name).To(e => e.Name));
    }
}
