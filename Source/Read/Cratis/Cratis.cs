// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Infrastructure;

namespace Read.Cratis;

[Route("/api/cratis")]
public class Cratis : Controller
{
    [HttpGet("versions")]
    public async Task<IEnumerable<SemanticVersion>> CratisVersions()
    {
        var versions = await DockerHub.GetVersionsOfImage("aksioinsurtech", "cratis", 25);
        return versions.Select(version => new SemanticVersion(version.Major, version.Minor, version.Patch, version.Prerelease, version.Metadata));
    }
}
