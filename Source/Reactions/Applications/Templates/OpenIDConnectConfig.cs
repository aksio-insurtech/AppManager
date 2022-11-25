// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions.Applications.Templates;

public record OpenIDConnectConfig(
    string Callback,
    string Issuer,
    string AuthorizationEndpoint,
    string TokenEndpoint,
    string ClientId,
    string ClientSecret,
    string LoginUrl,
    string AuthName)
{
    public static readonly OpenIDConnectConfig Empty = new(
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty);
}
