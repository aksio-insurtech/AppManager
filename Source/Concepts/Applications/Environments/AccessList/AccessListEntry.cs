// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications.Environments.AccessList;

/// <summary>
/// An entry for the access list.
/// /// </summary>
/// <param name="Name">The name.</param>
/// <param name="Address">The IP-address.</param>
public record AccessListEntry(AccessListName Name, AccessListAddress Address);
