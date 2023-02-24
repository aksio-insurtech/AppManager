// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Microsoft.Extensions.Logging;

namespace Reactions.Applications;

public static partial class ApplicationResourcesCoordinatorLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Deployment of '{Environment}' for application '{ApplicationName}' started")]
    public static partial void DeploymentStarted(this ILogger logger, ApplicationEnvironmentName environment, ApplicationName applicationName);

    [LoggerMessage(1, LogLevel.Information, "Changing image on deployable '{DeployableName}' to '{ImageName}' on microservice '{MicroserviceName}' in '{Environment}' for application '{ApplicationName}' using Pulumi")]
    public static partial void ChangingDeployableImage(this ILogger logger, MicroserviceName microserviceName, DeployableName deployableName, DeployableImageName imageName, ApplicationEnvironmentName environment, ApplicationName applicationName);
}
