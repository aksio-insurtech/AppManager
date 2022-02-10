// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Organizations;

namespace Read.Organizations
{
    public class OrganizationsProjection : IProjectionFor<Organization>
    {
        public ProjectionId Identifier => "5398c7f5-2baf-494b-abac-0a35eeb5fd98";

        public void Define(IProjectionBuilderFor<Organization> builder) => builder
            .From<OrganizationRegistered>(_ => _
                .Set(m => m.Name).To(e => e.Name));
    }
}
