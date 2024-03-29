// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using MongoDB.Bson;

namespace Infrastructure;

public record StackDeploymentForMicroservice(MicroserviceId Id, string Environment, BsonDocument Deployment);
