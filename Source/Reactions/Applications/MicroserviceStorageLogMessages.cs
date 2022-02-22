// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Reactions.Applications;

public static partial class MicroserviceStorageLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Uploading '{FileName}' for microservice '{Microservice}' in application '{application}' to file share '{Share}'")]
    public static partial void Uploading(this ILogger logger, string application, string microservice, string fileName, string share);
}
