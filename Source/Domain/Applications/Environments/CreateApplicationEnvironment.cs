// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;
using Concepts.Azure;

namespace Domain.Applications.Environments;

public record CreateApplicationEnvironment(
    ApplicationEnvironmentId EnvironmentId,
    ApplicationEnvironmentName Name,
    ApplicationEnvironmentDisplayName DisplayName,
    ApplicationEnvironmentShortName ShortName,
    AzureSubscriptionId AzureSubscriptionId,
    CloudLocationKey CloudLocation);
