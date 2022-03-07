// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;

namespace Reactions.Applications;

public static class TagsExtensions
{
    public static Tags GetTags(this Application application, CloudRuntimeEnvironment environment)
    {
        return new Tags(new Dictionary<string, string>
                {
                        { "application", application.Id.ToString() },
                        { "applicationName", application.Name.Value },
                        { "environment", Enum.GetName(typeof(CloudRuntimeEnvironment), environment) ?? string.Empty }
                });
    }
}
