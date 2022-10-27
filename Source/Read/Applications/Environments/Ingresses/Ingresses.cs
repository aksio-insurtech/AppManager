// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments.Ingresses;

namespace Read.Applications.Environments.Ingresses;

[Route("/api/applications/{applicationId}/environments/{environmentId}/ingresses")]
public class Ingresses : Controller
{
    readonly IMongoCollection<Ingress> _collection;

    public Ingresses(IMongoCollection<Ingress> collection)
    {
        _collection = collection;
    }

    [HttpGet("{ingressId}")]
    public Task<ClientObservable<Ingress>> IngressById([FromRoute] IngressId ingressId) =>
        _collection.ObserveId(ingressId);
}
