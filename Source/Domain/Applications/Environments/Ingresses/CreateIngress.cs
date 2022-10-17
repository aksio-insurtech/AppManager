// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Ingresses;

namespace Domain.Applications.Environments.Ingresses;

public record CreateIngress(IngressId IngressId, IngressName Name);
