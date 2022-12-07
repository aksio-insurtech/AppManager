// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions.Applications.Templates;

public record TenantConfig(string TenantId, string Domain, string OnBehalfOf, IEnumerable<string> TenantIdClaims);
