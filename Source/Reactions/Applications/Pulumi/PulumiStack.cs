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
public record PulumiStack(WorkspaceStack Stack, JsonNode Json)
{
    /// <summary>
    /// Check if stack has a resource group defined.
    /// </summary>
    /// <returns>True if it has, false if not.</returns>
    public bool HasResourceGroup()
    {
        var resourceGroup = Json["deployment"]!.AsObject()["resources"]!.AsArray().FirstOrDefault(_ => _!["type"]!.GetValue<string>().Equals("azure-native:resources:ResourceGroup"));
        var external = resourceGroup!["external"];
        if (external is not null)
        {
            return !external.GetValue<bool>();
        }
        return resourceGroup is not null;
    }
}
