// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Infrastructure;

public static partial class StacksLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Saving stack for {applicationId}")]
    public static partial void Saving(this ILogger logger, ApplicationId applicationId);

    [LoggerMessage(1, LogLevel.Information, "Getting stack for {applicationId}")]
    public static partial void Getting(this ILogger logger, ApplicationId applicationId);

    [LoggerMessage(2, LogLevel.Error, "Problems saving for {applicationId}")]
    public static partial void ErrorSaving(this ILogger logger, ApplicationId applicationId, Exception exception);
}
