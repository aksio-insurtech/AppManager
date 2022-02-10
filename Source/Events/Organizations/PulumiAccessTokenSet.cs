// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Pulumi;

namespace Events.Organizations
{
    [EventType("f78e087a-1f10-45b8-89a6-1068bca23ae2")]
    public record PulumiAccessTokenSet(PulumiAccessToken AccessToken);
}
