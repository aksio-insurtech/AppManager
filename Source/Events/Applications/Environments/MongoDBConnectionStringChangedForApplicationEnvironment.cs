// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.MongoDB;

namespace Events.Applications.Environments;

[EventType("1ce9b7fb-f65b-456d-ac9e-c20c86e2b9d8")]
public record MongoDBConnectionStringChangedForApplicationEnvironment(MongoDBConnectionString ConnectionString);
