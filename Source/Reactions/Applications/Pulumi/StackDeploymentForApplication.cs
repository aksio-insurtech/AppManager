// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MongoDB.Bson;

namespace Reactions.Applications.Pulumi;

public record StackDeploymentForApplication(ApplicationId Id, BsonDocument Deployment);
