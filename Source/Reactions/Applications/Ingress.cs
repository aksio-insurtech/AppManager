// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Ingresses;

namespace Reactions.Applications;

public record Ingress(IngressId Id, IngressName Name, IEnumerable<Route> Routes);