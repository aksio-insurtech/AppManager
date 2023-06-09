// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Concepts.Applications;
using Concepts.Applications.Environments.AccessList;
using Concepts.Applications.Environments.Ingresses;
using Concepts.Security;
using Reactions.Applications.Templates;

namespace Reactions.Applications;

/// <summary>
/// An ingress.
/// </summary>
/// <param name="Id">The ingress id.</param>
/// <param name="Name">The name.</param>
/// <param name="Resolver">If path is a regular expression, you need to specify a (dns) resolver. For eaxmple google's 8.8.8.8.</param>
/// <param name="MiddlewareVersion">Version of the aksioinsurtech/ingressmiddleware container image to use.</param>
/// <param name="IdentityDetailsProvider">IdentityDetailsProvider.</param>
/// <param name="Domain">Domain/hostname information.</param>
/// <param name="AuthDomain">AuthDomain.</param>
/// <param name="Routes">List of routes for this ingress.</param>
/// <param name="IdentityProviders">List of identity providers.</param>
/// <param name="RouteTenantResolution">Route tenant resolution.</param>
/// <param name="OAuthBearerTokenProvider">The OAuth bearer tokenprovider, if applicable.</param>
/// <param name="AccessList">An list of IP addresses that should have access, used for the least secure authentication.</param>
public record Ingress(
    IngressId Id,
    IngressName Name,
    Resolver? Resolver,
    SemanticVersion MiddlewareVersion,
    MicroserviceId IdentityDetailsProvider,
    Domain? Domain,
    Domain? AuthDomain,
    IEnumerable<Route> Routes,
    IEnumerable<IdentityProvider> IdentityProviders,
    RouteTenantResolution? RouteTenantResolution,
    OAuthBearerTokenProvider? OAuthBearerTokenProvider,
    IEnumerable<AccessListEntry> AccessList)
{
    public bool IsImpersonationEnabled => IdentityProviders.Any(identityProvider => identityProvider.Impersonation is not null);

    public Impersonation? GetImpersonationTemplateContent(ApplicationEnvironmentWithArtifacts environment) => IsImpersonationEnabled ?
            new Impersonation(
                IdentityProviders.Select(_ => _.Name.Value),
                environment.Tenants.Select(_ => _.Id.ToString()),
                IdentityProviders.SelectMany(_ => _.Impersonation?.Roles ?? Enumerable.Empty<Role>()).Select(_ => _.Value),
                IdentityProviders.SelectMany(_ => _.Impersonation?.Groups ?? Enumerable.Empty<Group>()).Select(_ => _.Value),
                IdentityProviders.SelectMany(_ => _.Impersonation?.Claims ?? Enumerable.Empty<Claim>())) : null;
}
