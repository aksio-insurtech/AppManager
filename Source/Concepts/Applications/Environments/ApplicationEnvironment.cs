// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Ingresses;

namespace Concepts.Applications.Environments;

public record ApplicationEnvironment(
    ApplicationEnvironmentId Id,
    ApplicationEnvironmentName Name,
    ApplicationEnvironmentDisplayName DisplayName,
    ApplicationEnvironmentShortName ShortName,
    IEnumerable<Ingress> Ingresses)
{
    public Ingress GetIngressById(IngressId id) => Ingresses.First(_ => _.Id == id);
}
