// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Resources;

namespace Reactions.Applications.Pulumi.Resources;

public class ResourceResultsByType : Dictionary<ResourceTypeId, object>
{
    public void Register(
        ResourceTypeId type,
        object result) => Add(type, result);

    public bool HasFor(ResourceTypeId type) => ContainsKey(type);

    public TResult GetById<TResult>(ResourceTypeId type)
        where TResult : class
    {
        ThrowIfMissingResourceResultForTypeId(type);
        return (this[type] as TResult)!;
    }

    void ThrowIfMissingResourceResultForTypeId(ResourceTypeId type)
    {
        if (!ContainsKey(type))
        {
            throw new MissingResourceResultForResourceType($"No resource of type '{type}' was registered");
        }
    }
}
