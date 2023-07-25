// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Execution;
using Concepts.Applications.Environments;

namespace Read.Applications.Environments.Tenants;

public record TenantKey(ApplicationEnvironmentId EnvironmentId, TenantId TenantId);
