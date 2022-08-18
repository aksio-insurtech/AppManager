// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts;

public static class CloudRuntimeEnvironmentExtensions
{
    public static string ToDisplayName(this CloudRuntimeEnvironment environment) =>
        environment switch
        {
            CloudRuntimeEnvironment.Production => "prod",
            CloudRuntimeEnvironment.Development => "dev",
            _ => "default"
        };

    public static string ToShortName(this CloudRuntimeEnvironment environment) =>
        environment switch
        {
            CloudRuntimeEnvironment.Development => "D",
            CloudRuntimeEnvironment.Production => "D",
            _ => "U"
        };
}
