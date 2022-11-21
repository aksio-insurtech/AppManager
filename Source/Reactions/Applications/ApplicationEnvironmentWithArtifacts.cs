// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Concepts.Applications;
using Concepts.Applications.Environments;
using Concepts.Applications.Environments.Ingresses;
using Concepts.Azure;

namespace Reactions.Applications;

public record ApplicationEnvironmentWithArtifacts(
    ApplicationEnvironmentId Id,
    ApplicationEnvironmentName Name,
    ApplicationEnvironmentDisplayName DisplayName,
    ApplicationEnvironmentShortName ShortName,
    SemanticVersion CratisVersion,
    AzureSubscriptionId AzureSubscriptionId,
    CloudLocationKey CloudLocation,
    ApplicationResources Resources,
    IEnumerable<Certificate> Certificates,
    IEnumerable<Tenant> Tenants,
    IEnumerable<Ingress> Ingresses,
    IEnumerable<Microservice> Microservices) : ApplicationEnvironment(Id, Name, DisplayName, ShortName)
{
    public Ingress GetIngressById(IngressId id) => Ingresses.Single(_ => _.Id == id);
    public Microservice GetMicroserviceById(MicroserviceId id) => Microservices.Single(_ => _.Id == id);
}
