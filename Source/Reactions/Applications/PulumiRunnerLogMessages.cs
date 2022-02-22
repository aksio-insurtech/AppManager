// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Reactions.Applications;

public static partial class PulumiRunnerLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Exporting stack to look for pending operations")]
    public static partial void ExportStack(this ILogger logger);

    [LoggerMessage(1, LogLevel.Information, "Importing stack after removing any pending operations")]
    public static partial void ImportStack(this ILogger logger);
}
