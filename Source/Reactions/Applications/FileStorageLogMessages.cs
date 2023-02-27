// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Reactions.Applications;

internal static partial class FileStorageLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Uploading '{FileName}' to file share '{Share}'")]
    internal static partial void Uploading(this ILogger<FileStorage> logger, string fileName, string share);

    [LoggerMessage(1, LogLevel.Warning, "File '{FileName}' does not have any content - ignoring upload to '{Share}'")]
    internal static partial void NoContent(this ILogger<FileStorage> logger, string fileName, string share);
}
