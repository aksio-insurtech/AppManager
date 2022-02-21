// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts
{
    public static class CloudRuntimeEnvironmentExtensions
    {
        public static string GetStackNameFor(this CloudRuntimeEnvironment environment) =>
            environment switch
            {
                CloudRuntimeEnvironment.Production => "prod",
                CloudRuntimeEnvironment.Development => "dev",
                _ => "default"
            };
    }
}
