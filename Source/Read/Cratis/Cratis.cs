// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Infrastructure;

namespace Read.Cratis;

[Route("/api/cratis")]
public class Cratis : Controller
{
    readonly IDockerHub _dockerHub;

    public Cratis(IDockerHub dockerHub)
    {
        _dockerHub = dockerHub;
    }

    [HttpGet("versions")]
    public Task<IEnumerable<SemanticVersion>> CratisVersions() => _dockerHub.GetVersionsOfCratis();
}
