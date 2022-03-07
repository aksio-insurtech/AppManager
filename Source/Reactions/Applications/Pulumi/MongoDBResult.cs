// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.Mongodbatlas;

namespace Reactions.Applications.Pulumi;

public record MongoDBResult(Cluster Cluster, string ConnectionString, string Password);
