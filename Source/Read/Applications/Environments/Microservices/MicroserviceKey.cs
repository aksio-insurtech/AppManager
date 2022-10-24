// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Concepts.Applications.Environments;

namespace Read.Applications.Environments.Microservices;

public record MicroserviceKey(ApplicationId ApplicationId, ApplicationEnvironmentId EnvironmentId, MicroserviceId MicroserviceId);
