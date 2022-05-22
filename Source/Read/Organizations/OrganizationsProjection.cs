// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Organizations;

namespace Read.Organizations;

public class OrganizationsProjection : IProjectionFor<Organization>
{
    public ProjectionId Identifier => "b72421d7-d62c-4cef-9f55-62b1e096c28a";

    public void Define(IProjectionBuilderFor<Organization> builder) => builder
        .From<OrganizationRegistered>(_ => _
            .Set(m => m.Name).To(e => e.Name));
}
