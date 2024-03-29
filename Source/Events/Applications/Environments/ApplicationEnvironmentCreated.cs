// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Concepts.Applications;
using Concepts.Applications.Environments;
using Concepts.Azure;

namespace Events.Applications.Environments;

[EventType("f98840bd-9c98-456f-821e-d9840ab846ae")]
public record ApplicationEnvironmentCreated(
    ApplicationId ApplicationId,
    ApplicationEnvironmentName Name,
    ApplicationEnvironmentDisplayName DisplayName,
    ApplicationEnvironmentShortName ShortName,
    SemanticVersion CratisVersion,
    AzureSubscriptionId AzureSubscriptionId,
    CloudLocationKey CloudLocation);
