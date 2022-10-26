// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;

namespace Read.Applications.Environments;

public class CustomDomainsForApplicationEnvironmentProjection : IProjectionFor<CustomDomainsForApplicationEnvironment>
{
    public ProjectionId Identifier => "442e4dd2-6a3b-49a0-a981-adbc55bb4fa7";

    public void Define(IProjectionBuilderFor<CustomDomainsForApplicationEnvironment> builder) => builder
        .Children(m => m.Domains, _ => _
            .IdentifiedBy(m => m.Domain)
            .From<CustomDomainAddedToApplicationEnvironment>(_ => _
                .UsingKey(m => m.Domain)
                .Set(m => m.Certificate).To(e => e.Certificate)));
}
