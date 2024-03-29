// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.MongoDB;

namespace Events.Settings;

[EventType("6b106aac-5ec6-43b9-af3c-c7b631cfa64d")]
public record MongoDBSettingsSet(MongoDBOrganizationId OrganizationId, MongoDBPublicKey PublicKey, MongoDBPrivateKey PrivateKey);
