// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments;

namespace Read.Applications.Environments.Tenants;

public class TenantProjection : IProjectionFor<Tenant>
{
    public ProjectionId Identifier => "d2459761-2d11-4008-94e3-832d5e9a58a0";

    public void Define(IProjectionBuilderFor<Tenant> builder) => builder
        .From<TenantAddedToApplicationEnvironment>(_ => _
            .Set(m => m.EnvironmentId).ToEventSourceId()
            .Set(m => m.Name).To(e => e.Name)
            .Set(m => m.ShortName).To(e => e.ShortName));
}
