// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Ingresses;

namespace Read.Applications.Environments.Ingresses;

// TODO: Change to composite key when Cratis supports it (https://github.com/aksio-insurtech/Cratis/issues/551)
public record Ingress(IngressId Id, IngressName Name, IEnumerable<IngressRoute> Routes);
