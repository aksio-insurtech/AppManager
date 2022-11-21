// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Reactions.Applications;

public static class TagsExtensions
{
    public static Tags GetTags(this Application application, ApplicationEnvironment environment)
    {
        return new Tags(new Dictionary<string, string>
                {
                        { "ApplicationId", application.Id.ToString() },
                        { "Application", application.Name.Value },
                        { "Environment", environment.Name }
                });
    }
}
