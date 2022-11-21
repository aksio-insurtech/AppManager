// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;

namespace Infrastructure;

public interface IDockerHub
{
    Task<SemanticVersion> GetLatestVersionOfImage(string organization, string image);
    Task<IEnumerable<SemanticVersion>> GetVersionsOfImage(string organization, string image, int numberOfVersions);
}
