// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions.Applications.Templates;

public record IngressMiddlewareTemplateContent(
    bool AzureAd,
    bool IdPorten,
    OpenIDConnectConfig AzureAdConfig,
    OpenIDConnectConfig IdPortenConfig,
    IEnumerable<TenantConfig> Tenants);
