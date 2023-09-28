// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Reactions.Applications.Templates;

namespace Reactions.Applications;

public static class IdentityProviderExtensions
{
    /// <summary>
    /// Make an <see cref="AuthorizationConfig"/> object from this <see cref="IdentityProvider"/>.
    /// Sets audience to 'api://$clientId' for app roles.
    /// </summary>
    /// <param name="identityProvider">The <see cref="IdentityProvider"/> to convert.</param>
    /// <returns>A <see cref="IngressMiddlewareAuthorizationConfig"/> representation.</returns>
    /// <exception cref="ConfigurationError">Thrown if any roles have type=undefined.</exception>
    public static IEnumerable<IngressMiddlewareAuthorizationConfig> IngressMiddlewareAuthorizationConfig(
        this IdentityProvider identityProvider)
    {
        // If no auth is required, set this up for both user and app audience.
        if (identityProvider.Authorization?.NoAuthorizationRequired ?? false)
        {
            yield return new(
                $"api://{identityProvider.ClientId}",
                Array.Empty<string>(),
                identityProvider.Authorization?.NoAuthorizationRequired ?? false);
            yield return new(
                identityProvider.ClientId,
                Array.Empty<string>(),
                identityProvider.Authorization?.NoAuthorizationRequired ?? false);
        }

        var roles = identityProvider.Authorization?.Roles?.ToList() ?? new List<AuthorizationRole>();
        if (!roles.Any())
        {
            yield break;
        }

        if (roles.Any(r => r.Type == AuthorizationRoleTypes.Undefined))
        {
            throw new ConfigurationError(
                $"Identity provider {identityProvider.Id}, for client {identityProvider.ClientId} has an invalid role type defined!");
        }

        // If this has any app roles, return appropriate auth config.
        var appRoles = roles.Where(r => r.Type == AuthorizationRoleTypes.App).ToList();
        if (appRoles.Any())
        {
            yield return new(
                $"api://{identityProvider.ClientId}",
                appRoles.Select(r => r.Role),
                identityProvider.Authorization?.NoAuthorizationRequired ?? false);
        }

        // If this has any user roles, return appropriate auth config.
        var userRoles = roles.Where(r => r.Type == AuthorizationRoleTypes.User).ToList();
        if (userRoles.Any())
        {
            yield return new(
                identityProvider.ClientId,
                userRoles.Select(r => r.Role),
                identityProvider.Authorization?.NoAuthorizationRequired ?? false);
        }
    }
}