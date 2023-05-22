// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments.Ingresses;
using Reactions.Applications;

namespace Bootstrap;

/// <summary>
/// Exception that gets thrown when there is a ingress defined which does not have any authentication solutions defined.
/// You need to use IdentityProviders, OAuthBearerTokenProvider or AccessList.
/// </summary>
public class IngressDoesNotHaveAnyAuthenticationDefined : Exception
{
    /// <summary>
    /// Ingress id.
    /// </summary>
    public IngressId Id { get; }

    /// <summary>
    /// Ingress name.
    /// </summary>
    public IngressName Name { get; }

    public IngressDoesNotHaveAnyAuthenticationDefined(Ingress ingress)
        : base($"Missing requirement: Ingress {ingress.Name} (id {ingress.Id}) does not have any authentication solution defined. You need to use IdentityProviders, OAuthBearerTokenProvider or AccessList")
    {
        Id = ingress.Id;
        Name = ingress.Name;
    }
}