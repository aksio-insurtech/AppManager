// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Events.Applications;

[EventType("f98840bd-9c98-456f-821e-d9840ab846ae")]
public record EnvironmentCreated(ApplicationId ApplicationId, ApplicationEnvironmentName Name, ApplicationEnvironmentDisplayName DisplayName, ApplicationEnvironmentShortName ShortName);
