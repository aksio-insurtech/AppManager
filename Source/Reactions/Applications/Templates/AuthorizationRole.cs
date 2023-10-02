// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

namespace Reactions.Applications.Templates;

/// <summary>
/// Defines a authorization role.
/// </summary>
/// <param name="Type">The role type (app or user).</param>
/// <param name="Role">The role value.</param>
public record AuthorizationRole([property: JsonConverter(typeof(JsonStringEnumConverter))] AuthorizationRoleTypes Type, string Role);