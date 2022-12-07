// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions.Applications.Templates;

public record OpenIDConnectConfig(
    string Issuer,
    string AuthorizationEndpoint)
{
    public static readonly OpenIDConnectConfig Empty = new(
        string.Empty,
        string.Empty);
}
