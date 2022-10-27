// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Concepts.Applications.Environments.Ingresses;

namespace Events.Applications.Environments.Ingresses;

[EventType("7cc944f1-dda3-4898-8467-d35354f00d78")]
public record IngressCreated(ApplicationId ApplicationId, ApplicationEnvironmentId EnvironmentId, IngressName Name);
