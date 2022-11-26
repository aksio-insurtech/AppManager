// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Reactions.Applications;

public record Certificate(CertificateId Id, CertificateName Name, CertificateValue Value, CertificatePassword Password);