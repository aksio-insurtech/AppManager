// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments.Ingresses;

namespace Read.Applications.Environments.Ingresses;

public class IngressProjection : IProjectionFor<Ingress>
{
    public ProjectionId Identifier => "be94702a-5420-4ecc-98b3-6c2405b5e1a7";

    public void Define(IProjectionBuilderFor<Ingress> builder) => builder
        .From<IngressCreated>(_ => _
            .Set(m => m.Name).To(e => e.Name))
        .Children(m => m.CustomDomains, _ => _
            .IdentifiedBy(m => m.Domain)
            .From<CustomDomainAddedToIngress>(_ => _
                .UsingKey(e => e.Domain)
                .Set(m => m.CertificateId).To(e => e.CertificateId)))
        .Children(m => m.Routes, _ => _
            .IdentifiedBy(m => m.Path)
            .From<RouteDefinedOnIngress>(_ => _
                .UsingKey(e => e.Path)
                .Set(m => m.TargetMicroservice).To(e => e.TargetMicroservice)
                .Set(m => m.TargetPath).To(e => e.TargetPath)));
}
