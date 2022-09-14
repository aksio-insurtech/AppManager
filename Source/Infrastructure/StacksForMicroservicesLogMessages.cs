// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public static partial class StacksForMicroservicesLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Saving stack for microservice with identifier '{microserviceId}'")]
    public static partial void Saving(this ILogger<StacksForMicroservices> logger, MicroserviceId microserviceId);

    [LoggerMessage(1, LogLevel.Information, "Getting stack for microservice with identifier '{microserviceId}'")]
    public static partial void Getting(this ILogger<StacksForMicroservices> logger, MicroserviceId microserviceId);

    [LoggerMessage(2, LogLevel.Error, "Problems saving for microservice with identifier '{microserviceId}'")]
    public static partial void ErrorSaving(this ILogger<StacksForMicroservices> logger, MicroserviceId microserviceId, Exception exception);
}
