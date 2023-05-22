// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Reactions.Applications;

namespace Bootstrap;

public static class IngressExtensions
{
    /// <summary>
    /// Checks if this ingress has any authentication methods configured.
    /// </summary>
    /// <param name="ingress">The ingress to check.</param>
    /// <returns>True if this ingress has any authentication methods defined, false if none are detected.</returns>
    /// <exception cref="InvalidAuthentication">Is thrown if the user attempts something we know won't work. Exception message should help identify the problem.</exception>
    public static bool HasAuthentication(this Ingress ingress)
    {
        // Any identity providers (such as AAD)?
        if (ingress.IdentityProviders.Any())
        {
            return true;
        }

        // Any OAuth Bearertoken interpretation (such as Maskinporten)?
        if (!string.IsNullOrWhiteSpace(ingress.OAuthBearerTokenProvider?.Authority?.Value))
        {
            return true;
        }

        // Any IP addresses in the access list?
        if (ingress.AccessList.Any())
        {
            return true;
        }

        // If no methods are used, the ingress does not have an acceptable configuration.
        return false;
    }

    /// <summary>
    /// Checks some obvious configuration errors.
    /// </summary>
    /// <param name="ingress">The ingress to check.</param>
    /// <exception cref="InvalidAuthentication">Is thrown if the user attempts something we know won't work. Exception message should help identify the problem.</exception>
    public static void ValidateAuthentication(this Ingress ingress)
    {
        // Is the user trying IdentityProvider AND OauthBearer token? That won't work, so fail to help user.
        if (ingress.IdentityProviders.Any() && !string.IsNullOrWhiteSpace(ingress.OAuthBearerTokenProvider?.Authority?.Value))
        {
            throw new InvalidAuthentication(
                "You cannot have both IdentityProvider AND OaiuthBearerTokenProvider, if you need both you will have to create two ingresses");
        }
    }
}
