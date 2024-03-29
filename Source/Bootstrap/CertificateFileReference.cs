// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Bootstrap;

public record CertificateFileReference(CertificateId Id, string File, CertificatePassword Password);
