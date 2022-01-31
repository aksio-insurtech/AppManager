// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Organizations;

namespace Events.Organizations
{
    [EventType("36d9ca76-c0b2-4461-bed9-4fe1d89c7bec")]
    public record OrganizationRegistered(OrganizationName Name);
}
