// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Concepts.Applications.Environments.Ingresses;

namespace Read.Applications.Environments.Ingresses;

public record IngressKey(ApplicationId ApplicationId, ApplicationEnvironmentId EnvironmentId, IngressId IngressId);
