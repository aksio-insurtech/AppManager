// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Read.Applications.Environments;

public record CustomDomainsForApplicationEnvironment(ApplicationEnvironmentId Id, IEnumerable<CustomDomain> Domains);
