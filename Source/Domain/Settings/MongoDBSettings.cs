// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.MongoDB;

namespace Domain.Settings;

public record MongoDBSettings(MongoDBOrganizationId OrganizationId, MongoDBPublicKey PublicKey, MongoDBPrivateKey PrivateKey);
