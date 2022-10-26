// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.ManagedIdentity;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationUserAssignedIdentityExtensions
{
    public static UserAssignedIdentity SetupUserAssignedIdentity(this Application application, ApplicationEnvironmentWithArtifacts environment, ResourceGroup resourceGroup, Tags tags)
    {
        var identityName = $"{application.Name}-user";
        return new UserAssignedIdentity(identityName, new()
        {
            Location = environment.CloudLocation.Value,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
        });
    }
}
