// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions.Applications;

/// <summary>
/// The Mutual TLS / Client Certificate configuration.
/// </summary>
/// <param name="AuthorityCertificate">Base64 representation of the certificate to validate client certificates against.</param>
/// <param name="AcceptedSerialNumbers">The list of accepted client certificate serial-numbers.</param>
public record MutualTLS(string AuthorityCertificate, IEnumerable<string> AcceptedSerialNumbers);