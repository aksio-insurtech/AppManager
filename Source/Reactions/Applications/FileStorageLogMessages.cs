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

    [LoggerMessage(
        2,
        LogLevel.Information,
        "File '{FileName}' on file share '{Share}' already existed with the exact same content, skipping upload")]
    internal static partial void SkippingUploadOfIdenticalFile(this ILogger<FileStorage> logger, string fileName, string share);

    [LoggerMessage(
        3,
        LogLevel.Error,
        "'The specified resource may be in use by an SMB client' exception when uploading file '{FileName}' to file share '{Share}'. Will retry once")]
    internal static partial void RetryingUpload(
        this ILogger<FileStorage> logger,
        Exception requestFailedException,
        string fileName,
        string share);

    [LoggerMessage(4, LogLevel.Error, "Exception when uploading file '{FileName}' to file share '{Share}'. Will not retry again")]
    internal static partial void FailedRetry(
        this ILogger<FileStorage> logger,
        Exception requestFailedException,
        string fileName,
        string share);

    [LoggerMessage(
        5,
        LogLevel.Error,
        "Unknown exception when uploading file '{FileName}' to file share '{Share}'. Will not retry")]
    internal static partial void UnknownExceptionWhileUploading(
        this ILogger<FileStorage> logger,
        Exception requestFailedException,
        string fileName,
        string share);

    [LoggerMessage(
        6,
        LogLevel.Error,
        "Request failed exception when uploading file '{FileName}' to file share '{Share}'. Will not retry")]
    internal static partial void RequestFailedExceptionWhileUploading(
        this ILogger<FileStorage> logger,
        Exception requestFailedException,
        string fileName,
        string share);
}