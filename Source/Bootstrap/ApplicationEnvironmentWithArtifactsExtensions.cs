// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Reactions.Applications;

namespace Bootstrap;

/// <summary>
/// ApplicationAndEnvironment extensions.
/// </summary>
public static class ApplicationEnvironmentWithArtifactsExtensions
{
    /// <summary>
    /// Validates the configuration, throws exceptions on failure.
    /// </summary>
    /// <param name="environment">The environment to validate.</param>
    /// <exception cref="EnvironmentStorageAccessListEntryDoesNotHaveAnIpAddress">Thrown if there is a storage accesslist definition without an actual ip-address.</exception>
    /// <exception cref="IngressDoesNotHaveAnyAuthenticationDefined">Thrown if any ingress is missing authentication.</exception>
    /// <exception cref="InvalidAuthentication">Thrown if the user attempts ingress auth config we know won't work. Exception message should help identify the problem.</exception>
    /// <exception cref="IngressIsMissingResolver">Thrown if any ingress routes is set to use a resolver, but none is defined.</exception>
    public static void ValidateConfiguration(this ApplicationEnvironmentWithArtifacts environment)
    {
        if (environment.Storage?.AccessList.Any(al => string.IsNullOrWhiteSpace(al.Address?.Value)) ?? false)
        {
            throw new EnvironmentStorageAccessListEntryDoesNotHaveAnIpAddress();
        }

        foreach (var ingress in environment.Ingresses)
        {
            ingress.ValidateAuthentication();

            if (!ingress.HasAuthentication())
            {
                throw new IngressDoesNotHaveAnyAuthenticationDefined(ingress);
            }

            if (ingress.Routes.Any(r => r.UseResolver) && string.IsNullOrWhiteSpace(ingress.Resolver?.Value))
            {
                throw new IngressIsMissingResolver(ingress);
            }
        }
    }
}