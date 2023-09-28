// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Concepts.Applications;
using Concepts.Applications.Environments.AccessList;
using Concepts.Applications.Environments.Ingresses;
using Concepts.Applications.Environments.Ingresses.IdentityProviders;
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
/// <param name="RouteTenantResolution">Optional Route tenant resolution to use.</param>
/// <param name="SpecifiedTenantResolution">Optional <see cref="SpecifiedTenantResolution"/> to use.</param>
/// <param name="OAuthBearerTokenProvider">The OAuth bearer tokenprovider, if applicable.</param>
/// <param name="AccessList">An list of IP addresses that should have access, used for the least secure authentication.</param>
/// <param name="MutualTLS">Mutual TLS / Client Certificate mode, if applicable. If accepted, this will relay any given certificate to the web application after vaildating it in the IngressMiddleware. If required the client must present a certificate with a valid serialnumber.</param>
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
    SpecifiedTenantResolution? SpecifiedTenantResolution,
    OAuthBearerTokenProvider? OAuthBearerTokenProvider,
    IEnumerable<AccessListEntry>? AccessList,
    MutualTLS? MutualTLS)
{
    public bool IsImpersonationEnabled => IdentityProviders.Any(identityProvider => identityProvider.Impersonation is not null);

    public MicroserviceId? GetTargetMicroserviceIdForImpersonation(ApplicationEnvironmentWithArtifacts environment) => IsImpersonationEnabled ?
        IdentityProviders.First(_ => _.Impersonation is not null).Impersonation!.TargetMicroservice : null;

    public ImpersonationMiddlewareConfig? GetImpersonationTemplateContent(ApplicationEnvironmentWithArtifacts environment) =>
        IsImpersonationEnabled
            ? new ImpersonationMiddlewareConfig(
                IdentityProviders.Select(_ => _.Name.Value),
                environment.Tenants.Select(_ => _.Id.ToString()),
                IdentityProviders.SelectMany(_ => _.Impersonation?.Roles ?? Enumerable.Empty<Role>()).Select(_ => _.Value),
                IdentityProviders.SelectMany(_ => _.Impersonation?.Groups ?? Enumerable.Empty<Group>()).Select(_ => _.Value),
                IdentityProviders.SelectMany(_ => _.Impersonation?.Claims ?? Enumerable.Empty<Claim>())
                    .Union(
                        IdentityProviders.Where(ip => ip.Type == IdentityProviderType.Azure)
                            .SelectMany(
                                _ => _.Authorization?.Roles.Select(role => new Claim("roles", role.Role)) ?? Array.Empty<Claim>())))
            : null;
}
