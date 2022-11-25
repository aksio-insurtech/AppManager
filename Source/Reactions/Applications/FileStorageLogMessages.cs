// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Reactions.Applications;

public static partial class FileStorageLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Uploading '{FileName}' to file share '{Share}'")]
    public static partial void Uploading(this ILogger<FileStorage> logger, string fileName, string share);

    [LoggerMessage(1, LogLevel.Warning, "File '{FileName}' does not have any content - ignoring upload to '{Share}'")]
    public static partial void NoContent(this ILogger<FileStorage> logger, string fileName, string share);
}
