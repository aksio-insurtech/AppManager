// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Events.Applications.Environments;

[EventType("f4db45b1-fce5-4f04-ab20-c9801bdb2e09")]
public record ConfigFileSetForApplicationEnvironment(ApplicationId ApplicationId, ApplicationEnvironmentId EnvironmentId, ConfigFileName Name, ConfigFileContent Content);
