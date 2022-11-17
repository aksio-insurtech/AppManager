// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments;

namespace Read.Applications.Environments.Tenants;

public class TenantProjection : IProjectionFor<Tenant>
{
    public ProjectionId Identifier => "d2459761-2d11-4008-94e3-832d5e9a58a0";

    public void Define(IProjectionBuilderFor<Tenant> builder) => builder
        .From<TenantAddedToApplicationEnvironment>(_ => _
            .UsingCompositeKey<TenantKey>(_ => _
                .Set(k => k.EnvironmentId).ToEventSourceId()
                .Set(k => k.TenantId).To(e => e.TenantId))
            .Set(m => m.Name).To(e => e.Name))
        .From<OnBehalfOfSetForTenant>(_ => _
            .UsingCompositeKey<TenantKey>(_ => _
                .Set(k => k.EnvironmentId).ToEventSourceId()
                .Set(k => k.TenantId).To(e => e.TenantId))
            .Set(m => m.OnBehalfOf).To(e => e.OnBehalfOf))
        .From<DomainAssociatedWithTenant>(_ => _
            .UsingCompositeKey<TenantKey>(_ => _
                .Set(k => k.EnvironmentId).ToEventSourceId()
                .Set(k => k.TenantId).To(e => e.TenantId))
            .Set(m => m.Domain).To(e => e.Domain)
            .Set(m => m.CertificateId).To(e => e.CertificateId));
}
