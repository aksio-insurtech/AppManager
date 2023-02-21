// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Events.Applications.Environments;

[EventType("e56c0661-9f6c-4641-a12e-3e51007a8941")]
public record ApplicationEnvironmentConsolidationFailed(ApplicationId ApplicationId, ApplicationEnvironmentId EnvironmentId, ApplicationEnvironmentConsolidationId ConsolidationId, IEnumerable<string> Errors, string StackTrace);
