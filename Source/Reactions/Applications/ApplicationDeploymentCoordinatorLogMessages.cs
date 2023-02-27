// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Microsoft.Extensions.Logging;

namespace Reactions.Applications;

internal static partial class ApplicationDeploymentCoordinatorLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Changing image on deployable '{DeployableId}' to '{ImageName}' on microservice '{MicroserviceId}' for application '{ApplicationName}' using Pulumi")]
    internal static partial void ChangingDeployableImage(this ILogger<ApplicationDeploymentCoordinator> logger, DeployableId deployableId, MicroserviceId microserviceId, DeployableImageName imageName, ApplicationName applicationName);
}
