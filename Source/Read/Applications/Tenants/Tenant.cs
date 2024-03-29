// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Concepts.Applications.Tenants;

namespace Read.Applications.Environments.Tenants;

public record Tenant(TenantKey Id, TenantName Name, OnBehalfOf OnBehalfOf, DomainName Domain, CertificateId CertificateId);
