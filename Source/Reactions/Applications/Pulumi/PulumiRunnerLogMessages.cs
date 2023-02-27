// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Reactions.Applications.Pulumi;

internal static partial class PulumiRunnerLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Exporting stack to look for pending operations")]
    internal static partial void ExportStack(this ILogger logger);

    [LoggerMessage(1, LogLevel.Information, "Importing stack after removing any pending operations")]
    internal static partial void ImportStack(this ILogger logger);

    [LoggerMessage(2, LogLevel.Information, "No pending operations in stack")]
    internal static partial void NoPendingOperations(this ILogger logger);
}
