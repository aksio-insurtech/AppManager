// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Resources;

namespace Reactions.Applications.Pulumi.Resources;

public class ResourceNotFound : Exception
{
    public ResourceNotFound(ResourceTypeId resourceTypeId)
        : base($"Resource '{resourceTypeId}' was not found")
    {
    }
}
