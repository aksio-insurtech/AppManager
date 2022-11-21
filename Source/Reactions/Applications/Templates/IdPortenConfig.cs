// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions.Applications.Templates;

public record IdPortenConfig(
    string Callback,
    string AuthorizationEndpoint,
    string TokenEndpoint,
    string ClientId,
    string ClientSecret,
    string AuthName)
{
    public static readonly IdPortenConfig Empty = new(
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty);
}
