// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Events.Applications.Environments;

[EventType("f8fdbbca-fed0-4c9c-83c9-ba15e2746bf9")]
public record ApplicationEnvironmentDeploymentStarted(ApplicationId ApplicationId, ApplicationEnvironmentId EnvironmentId, ApplicationEnvironmentDeploymentId DeploymentId);
