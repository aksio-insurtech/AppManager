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

    [LoggerMessage(6, LogLevel.Error, "Error while performing Pulumi Operation")]
    public static partial void Errored(this ILogger logger, Exception exception);

    [LoggerMessage(7, LogLevel.Information, "Pulumi Access token is '{AccessToken}'")]
    public static partial void PulumiInformation(this ILogger logger, string accessToken);

    [LoggerMessage(8, LogLevel.Information, "Creating or selecting stack")]
    public static partial void CreatingOrSelectingStack(this ILogger logger);

    [LoggerMessage(9, LogLevel.Information, "Removing pending operations")]
    public static partial void RemovingPendingOperations(this ILogger logger);

    [LoggerMessage(10, LogLevel.Information, "Installing plugins")]
    public static partial void InstallingPlugins(this ILogger logger);
}
