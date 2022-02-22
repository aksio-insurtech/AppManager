// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Reactions.Applications
{
    public static partial class ApplicationResourcesCoordinatorLogMessages
    {
        [LoggerMessage(0, LogLevel.Information, "Creating application '{ApplicationName}' using Pulumi")]
        public static partial void CreatingApplication(this ILogger logger, string applicationName);

        [LoggerMessage(1, LogLevel.Information, "Removing application '{ApplicationName}' using Pulumi")]
        public static partial void RemovingApplication(this ILogger logger, string applicationName);

        [LoggerMessage(2, LogLevel.Information, "Creating microservice '{MicroserviceName}' using Pulumi")]
        public static partial void CreatingMicroservice(this ILogger logger, string microserviceName);
        [LoggerMessage(3, LogLevel.Information, "Removing microservice '{MicroserviceName}' using Pulumi")]
        public static partial void RemovingMicroservice(this ILogger logger, string microserviceName);
    }
}
