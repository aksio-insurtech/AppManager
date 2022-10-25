// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Concepts;
using Semver;

namespace Infrastructure;

public class DockerHub : IDockerHub
{
    public async Task<IEnumerable<SemanticVersion>> GetVersionsOfImage(string organization, string image, int numberOfVersions)
    {
        var client = new HttpClient();
        var response = await client.GetStringAsync($"https://hub.docker.com/v2/repositories/{organization}/{image}/tags/?page_size={numberOfVersions}&page=1");
        var document = JsonDocument.Parse(response);
        return document.RootElement
            .GetProperty("results")
            .EnumerateArray()
            .Select(_ => _.GetProperty("name").GetString() ?? string.Empty)
            .Where(_ => !_.StartsWith("latest") && !_.Contains('-'))
            .Select(_ => SemVersion.Parse(_, SemVersionStyles.Any))
            .OrderByDescending(_ => _)
            .Select(_ => new SemanticVersion(_.Major, _.Minor, _.Patch, _.Prerelease, _.Metadata))
            .ToArray();
    }

    public async Task<SemanticVersion> GetLatestVersionOfImage(string organization, string image)
    {
        var versions = await GetVersionsOfImage(organization, image, 25);
        return versions.FirstOrDefault() ?? new SemanticVersion(0, 0, 0, string.Empty, string.Empty);
    }
}
