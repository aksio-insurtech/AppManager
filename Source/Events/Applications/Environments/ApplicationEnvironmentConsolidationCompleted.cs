// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Events.Applications.Environments;

[EventType("a3453bdf-b678-4118-b6ac-8b4d30ccee00")]
public record ApplicationEnvironmentConsolidationCompleted(ApplicationId ApplicationId, ApplicationEnvironmentId EnvironmentId, ApplicationEnvironmentConsolidationId ConsolidationId);
