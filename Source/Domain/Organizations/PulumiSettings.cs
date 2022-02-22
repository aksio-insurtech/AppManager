// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Pulumi;

namespace Domain.Organizations;

public record PulumiSettings(PulumiOrganization Organization, PulumiAccessToken AccessToken);
