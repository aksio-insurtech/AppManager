// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Applications.Environments;

namespace Events.Applications.Environments;

[EventType("5690a4dc-fadd-4ba1-ab23-8dc88ccf6eaf")]
public record SecretSetForApplicationEnvironment(ApplicationId ApplicationId, ApplicationEnvironmentId EnvironmentId, SecretKey Key, SecretValue Value);
