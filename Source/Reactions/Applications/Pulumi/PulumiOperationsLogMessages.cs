// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Reactions.Applications.Pulumi;

public static partial class PulumiOperationsLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Creating stack")]
    public static partial void CreatingStack(this ILogger logger);

    [LoggerMessage(1, LogLevel.Information, "Refreshing stack from cloud provider")]
    public static partial void RefreshingStack(this ILogger logger);

    [LoggerMessage(3, LogLevel.Information, "Up the stack")]
    public static partial void PuttingUpStack(this ILogger logger);

    [LoggerMessage(4, LogLevel.Information, "Down the stack")]
    public static partial void TakingDownStack(this ILogger logger);

    [LoggerMessage(5, LogLevel.Information, "Remove the stack")]
    public static partial void RemovingStack(this ILogger logger);
}
