// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Resources;

namespace Reactions.Applications.Pulumi.Resources;

public class MissingResourceResultForResourceType : Exception
{
    public MissingResourceResultForResourceType(ResourceTypeId type)
        : base($"No resource of type '{type}' was registered")
    {
    }
}
