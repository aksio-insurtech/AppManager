// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments.AccessList;

namespace Reactions.Applications;

/// <summary>
/// Extra storage configuration.
/// </summary>
/// <param name="AccessList">An list of IP addresses that should have access to the storage, in addition to the application environment network.</param>
public record StorageConfig(IEnumerable<AccessListEntry> AccessList);