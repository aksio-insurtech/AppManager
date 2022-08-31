// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Semver;

namespace Reactions.Applications.Pulumi;

public static class DockerHub
{
    public static async Task<string> GetLatestVersionOfImage(string organization, string image)
    {
        var client = new HttpClient();
        var response = await client.GetStringAsync($"https://hub.docker.com/v2/repositories/{organization}/{image}/tags/?page_size=25&page=1");
        var document = JsonDocument.Parse(response);
        var versions = document.RootElement
            .GetProperty("results")
            .EnumerateArray()
            .Select(_ => _.GetProperty("name").GetString() ?? string.Empty)
            .Where(_ => !_.StartsWith("latest") && !_.Contains('-'))
            .Select(_ => SemVersion.Parse(_, SemVersionStyles.Any))
            .OrderByDescending(_ => _)

            .ToArray();

        return versions.FirstOrDefault()!.ToString() ?? string.Empty;
    }
}