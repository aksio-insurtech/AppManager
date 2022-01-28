// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Events.Projections;
using Events.Applications;

namespace Read.Applications
{
    public class ApplicationsProjections : IProjectionFor<Application>
    {
        public ProjectionId Identifier => "5a52002b-f36e-4d69-8900-cd133de4aac3";

        public void Define(IProjectionBuilderFor<Application> builder) => builder
            .From<ApplicationCreated>(_ => _
                .Set(m => m.Name).To(e => e.Name)
                .Set(m => m.CloudLocation).To(e => e.CloudLocation));
    }
}
