// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Domain.Applications.Environments;

public record AddCertificateToApplicationEnvironment(CertificateId CertificateId, CertificateName Name, CertificateValue Certificate);
