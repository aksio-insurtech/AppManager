// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi.Resources;

public record RenderContext(
    Application Application,
    ApplicationEnvironmentWithArtifacts Environment,
    ResourceGroup ResourceGroup,
    Tags Tags,
    ResourceResultsByType Results,
    IEnumerable<Tenant> Tenants,
    IEnumerable<Microservice> Microservices);
