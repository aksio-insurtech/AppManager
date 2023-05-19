// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments.Ingresses;
using Reactions.Applications;

namespace Bootstrap;

/// <summary>
/// Exception that gets thrown when a ingress route needs the resolver, but none is defined.
/// </summary>
public class IngressIsMissingResolver : Exception
{
    /// <summary>
    /// Ingress id.
    /// </summary>
    public IngressId Id { get; }

    /// <summary>
    /// Ingress name.
    /// </summary>
    public IngressName Name { get; }

    public IngressIsMissingResolver(Ingress ingress)
        : base($"Missing requirement: Ingress {ingress.Name} (id {ingress.Id}) has a route that requries a resolver, but none is defined for this ingress")
    {
        Id = ingress.Id;
        Name = ingress.Name;
    }
}