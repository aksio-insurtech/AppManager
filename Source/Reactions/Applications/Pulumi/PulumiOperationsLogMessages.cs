// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Reactions.Applications.Pulumi;

internal static partial class PulumiOperationsLogMessages
{
    [LoggerMessage(0, LogLevel.Information, "Creating stack for '{application}'")]
    internal static partial void CreatingStack(this ILogger logger, string application);

    [LoggerMessage(1, LogLevel.Information, "Refreshing stack from providers")]
    internal static partial void RefreshingStack(this ILogger logger);

    [LoggerMessage(3, LogLevel.Information, "Up the stack")]
    internal static partial void PuttingUpStack(this ILogger logger);

    [LoggerMessage(4, LogLevel.Information, "Down the stack")]
    internal static partial void TakingDownStack(this ILogger logger);

    [LoggerMessage(5, LogLevel.Information, "Remove the stack")]
    internal static partial void RemovingStack(this ILogger logger);

    [LoggerMessage(6, LogLevel.Error, "Error while performing Pulumi Operation")]
    internal static partial void Errored(this ILogger logger, Exception exception);

    [LoggerMessage(7, LogLevel.Information, "Pulumi Access token is '{AccessToken}'")]
    internal static partial void PulumiInformation(this ILogger logger, string accessToken);

    [LoggerMessage(8, LogLevel.Information, "Creating or selecting stack")]
    internal static partial void CreatingOrSelectingStack(this ILogger logger);

    [LoggerMessage(9, LogLevel.Information, "Removing pending operations")]
    internal static partial void RemovingPendingOperations(this ILogger logger);

    [LoggerMessage(10, LogLevel.Information, "Installing plugins")]
    internal static partial void InstallingPlugins(this ILogger logger);

    [LoggerMessage(11, LogLevel.Information, "`Upping` stack")]
    internal static partial void UppingStack(this ILogger logger);

    [LoggerMessage(12, LogLevel.Information, "`Downing` stack")]
    internal static partial void DowningStack(this ILogger logger);

    [LoggerMessage(13, LogLevel.Information, "Removing stack")]
    internal static partial void StackBeingRemoved(this ILogger logger);

    [LoggerMessage(14, LogLevel.Information, "Setting all config")]
    internal static partial void SettingAllConfig(this ILogger logger);

    [LoggerMessage(15, LogLevel.Information, "Setting tags")]
    internal static partial void SettingsTags(this ILogger logger);

    [LoggerMessage(16, LogLevel.Information, "Getting existing stack deployment for '{application}'")]
    internal static partial void GettingStackDeployment(this ILogger logger, string application);

    [LoggerMessage(17, LogLevel.Information, "Saving stack deployment for application '{application}'")]
    internal static partial void SavingStackDeploymentForApplication(this ILogger logger, string application);

    [LoggerMessage(18, LogLevel.Information, "Saving stack deployment for microservice '{microservice}'")]
    internal static partial void SavingStackDeploymentForMicroservice(this ILogger logger, string microservice);
}
