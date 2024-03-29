// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Domain.Applications.Environments.Tenants;

public record AssociateDomainWithTenant(DomainName Domain, CertificateId CertificateId);
