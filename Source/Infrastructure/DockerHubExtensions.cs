// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;

namespace Infrastructure;

public static class DockerHubExtensions
{
    public const string AksioOrganization = "aksioinsurtech";
    public const string CratisImage = "cratis";
    public const string AppManagerImage = "app-manager";
    public const string IngressMiddlewareImage = "ingressmiddleware";

    public static Task<IEnumerable<SemanticVersion>> GetVersionsOfCratis(this IDockerHub dockerHub, int numberOfVersions = 25) => dockerHub.GetVersionsOfImage(AksioOrganization, CratisImage, numberOfVersions);
    public static Task<SemanticVersion> GetLastVersionOfCratis(this IDockerHub dockerHub) => dockerHub.GetLatestVersionOfImage(AksioOrganization, CratisImage);
    public static Task<SemanticVersion> GetLastVersionOfAppManager(this IDockerHub dockerHub) => dockerHub.GetLatestVersionOfImage(AksioOrganization, AppManagerImage);
    public static Task<SemanticVersion> GetLastVersionOfIngressMiddleware(this IDockerHub dockerHub) => dockerHub.GetLatestVersionOfImage(AksioOrganization, IngressMiddlewareImage);
}
