// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments;

namespace Read.Applications.Environments;

public class CertificatesForApplicationEnvironmentProjection : IProjectionFor<CertificatesForApplicationEnvironment>
{
    public ProjectionId Identifier => "a6f726aa-3191-416e-8258-e12fcb9c281c";

    public void Define(IProjectionBuilderFor<CertificatesForApplicationEnvironment> builder) => builder
        .Children(m => m.Certificates, _ => _
            .IdentifiedBy(m => m.CertificateId)
            .From<CertificateAddedToApplicationEnvironment>(_ => _
                .UsingParentKey(e => e.EnvironmentId)
                .UsingKey(e => e.CertificateId)
                .Set(m => m.Name).To(e => e.Name)));
}
