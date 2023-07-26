// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Execution;
using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Events.Applications.Environments;

[EventType("8590794e-0be5-4063-a98e-00b8f3b31f17")]
public record DomainAssociatedWithTenant(TenantId TenantId, DomainName Domain, CertificateId CertificateId);
