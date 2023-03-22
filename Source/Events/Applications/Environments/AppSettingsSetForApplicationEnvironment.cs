// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Events.Applications.Environments;

[EventType("9f4e2a05-f1e0-4ccb-ae32-97d952fc5c3b")]
public record AppSettingsSetForApplicationEnvironment(ApplicationId ApplicationId, ApplicationEnvironmentId EnvironmentId, AppSettingsContent Content);
