// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Concepts.Applications.Ingresses;

namespace Events.Applications.Environments.Ingresses;

[EventType("f078887c-362a-4893-9d88-2ed994ba3841")]
public record RouteDefinedOnIngress(ApplicationId ApplicationId, ApplicationEnvironmentId EnvironmentId, IngressId IngressId, string Path, MicroserviceId TargetMicroservice, string TargetPath);
