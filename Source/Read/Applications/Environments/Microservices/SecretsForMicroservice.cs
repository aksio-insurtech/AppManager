// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Read.Applications.Environments.Microservices;

public record SecretsForMicroservice(
    MicroserviceId Id,
    IEnumerable<Secret> Secrets);
