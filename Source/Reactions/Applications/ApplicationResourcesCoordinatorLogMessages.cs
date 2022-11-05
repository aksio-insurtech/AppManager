// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Reactions.Applications;

public static partial class ApplicationResourcesCoordinatorLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Creating application environment '{Environment}' for '{ApplicationName}' using Pulumi")]
    public static partial void CreatingApplicationEnvironment(this ILogger logger, string environment, string applicationName);

    [LoggerMessage(1, LogLevel.Information, "Removing application '{ApplicationName}' using Pulumi")]
    public static partial void RemovingApplication(this ILogger logger, string applicationName);

    [LoggerMessage(2, LogLevel.Information, "Creating microservice '{MicroserviceName}' in '{Environment}' for application '{ApplicationName}' using Pulumi")]
    public static partial void CreatingMicroservice(this ILogger logger, string microserviceName, string environment, string applicationName);

    [LoggerMessage(3, LogLevel.Information, "Removing microservice '{MicroserviceName}' from '{Environment}' for application '{ApplicationName}' using Pulumi")]
    public static partial void RemovingMicroservice(this ILogger logger, string microserviceName, string environment, string applicationName);

    [LoggerMessage(4, LogLevel.Information, "Adding '{DeployableName}' to microservice '{MicroserviceName}' in '{Environment}' for application '{ApplicationName}' with image '{ImageName}' using Pulumi")]
    public static partial void DeployableCreated(this ILogger logger, string microserviceName, string environment, string applicationName, string deployableName, string imageName);

    [LoggerMessage(5, LogLevel.Information, "Changing image on deployable '{DeployableName}' to '{ImageName}' on microservice '{MicroserviceName}' in '{Environment}' for application '{ApplicationName}' using Pulumi")]
    public static partial void ChangingDeployableImage(this ILogger logger, string microserviceName, string deployableName, string imageName, string environment, string applicationName);

    [LoggerMessage(6, LogLevel.Information, "Creating ingress '{IngressName}' in '{Environment}' for application '{ApplicationName}'")]
    public static partial void CreatingIngress(this ILogger logger, string ingressName, string environment, string applicationName);

    [LoggerMessage(7, LogLevel.Information, "Consolidation of '{Environment}' for application '{ApplicationName}' started")]
    public static partial void ConsolidationStarted(this ILogger logger, string environment, string applicationName);
}
