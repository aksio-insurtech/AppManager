// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Concepts.Applications.Environments.Ingresses;
using Microsoft.Extensions.Logging;

namespace Reactions.Applications;

public static partial class ApplicationResourcesCoordinatorLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Creating application environment '{Environment}' for '{ApplicationName}' using Pulumi")]
    public static partial void CreatingApplicationEnvironment(this ILogger logger, ApplicationEnvironmentName environment, ApplicationName applicationName);

    [LoggerMessage(1, LogLevel.Information, "Removing application '{ApplicationName}' using Pulumi")]
    public static partial void RemovingApplication(this ILogger logger, ApplicationName applicationName);

    [LoggerMessage(2, LogLevel.Information, "Creating microservice '{MicroserviceName}' in '{Environment}' for application '{ApplicationName}' using Pulumi")]
    public static partial void CreatingMicroservice(this ILogger logger, MicroserviceName microserviceName, ApplicationEnvironmentName environment, ApplicationName applicationName);

    [LoggerMessage(3, LogLevel.Information, "Removing microservice '{MicroserviceName}' from '{Environment}' for application '{ApplicationName}' using Pulumi")]
    public static partial void RemovingMicroservice(this ILogger logger, MicroserviceName microserviceName, ApplicationEnvironmentName environment, ApplicationName applicationName);

    [LoggerMessage(4, LogLevel.Information, "Adding '{DeployableName}' to microservice '{MicroserviceName}' in '{Environment}' for application '{ApplicationName}' with image '{ImageName}' using Pulumi")]
    public static partial void DeployableCreated(this ILogger logger, MicroserviceName microserviceName, ApplicationEnvironmentName environment, ApplicationName applicationName, DeployableName deployableName, DeployableImageName imageName);

    [LoggerMessage(5, LogLevel.Information, "Changing image on deployable '{DeployableName}' to '{ImageName}' on microservice '{MicroserviceName}' in '{Environment}' for application '{ApplicationName}' using Pulumi")]
    public static partial void ChangingDeployableImage(this ILogger logger, MicroserviceName microserviceName, DeployableName deployableName, DeployableImageName imageName, ApplicationEnvironmentName environment, ApplicationName applicationName);

    [LoggerMessage(6, LogLevel.Information, "Creating ingress '{IngressName}' in '{Environment}' for application '{ApplicationName}'")]
    public static partial void CreatingIngress(this ILogger logger, IngressName ingressName, ApplicationEnvironmentName environment, ApplicationName applicationName);

    [LoggerMessage(7, LogLevel.Information, "Consolidation of '{Environment}' for application '{ApplicationName}' started")]
    public static partial void ConsolidationStarted(this ILogger logger, ApplicationEnvironmentName environment, ApplicationName applicationName);
}
