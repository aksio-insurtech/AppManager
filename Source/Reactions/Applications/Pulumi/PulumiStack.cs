// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Nodes;
using Pulumi.Automation;

namespace Reactions.Applications.Pulumi;

/// <summary>
/// Represents a Pulumi stack and its current JSON representation.
/// </summary>
/// <param name="Stack">The actual <see cref="WorkspaceStack"/>.</param>
/// <param name="Json">The current JSON representation.</param>
public record PulumiStack(WorkspaceStack Stack, JsonNode Json);
