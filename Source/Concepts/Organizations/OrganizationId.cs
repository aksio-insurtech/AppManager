// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Organizations;

public record OrganizationId(Guid Value) : ConceptAs<Guid>(Value);
