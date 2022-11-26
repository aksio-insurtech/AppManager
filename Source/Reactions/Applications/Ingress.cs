// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Concepts.Applications;
using Concepts.Applications.Environments;
using Concepts.Applications.Environments.Ingresses;

namespace Reactions.Applications;

public record Ingress(
    IngressId Id,
    IngressName Name,
    SemanticVersion MiddlewareVersion,
    DomainName AuthDomain,
    CertificateId AuthCertificateId,
    IEnumerable<Route> Routes,
    IEnumerable<IdentityProvider> IdentityProviders);
