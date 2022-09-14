// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Infrastructure;

public static partial class StacksForApplicationsLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Saving stack for application with identifier '{applicationId}'")]
    public static partial void Saving(this ILogger<StacksForApplications> logger, ApplicationId applicationId);

    [LoggerMessage(1, LogLevel.Information, "Getting stack for application with identifier '{applicationId}'")]
    public static partial void Getting(this ILogger<StacksForApplications> logger, ApplicationId applicationId);

    [LoggerMessage(2, LogLevel.Error, "Problems saving for application with identifier '{applicationId}'")]
    public static partial void ErrorSaving(this ILogger<StacksForApplications> logger, ApplicationId applicationId, Exception exception);
}
