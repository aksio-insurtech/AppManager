// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Concepts.Applications.Tenants;

namespace Read.Applications;

public record TenantInEnvironment(TenantId TenantId, TenantName Name);
