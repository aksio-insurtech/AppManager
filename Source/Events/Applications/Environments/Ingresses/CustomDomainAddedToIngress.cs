// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Events.Applications.Environments.Ingresses;

[EventType("7651b7f1-79a0-4321-bab3-3d55a31c57ff")]
public record CustomDomainAddedToIngress(DomainName Domain, CertificateId CertificateId);
