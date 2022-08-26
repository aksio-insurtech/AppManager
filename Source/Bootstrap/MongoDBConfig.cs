// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.MongoDB;

namespace Bootstrap;

public record MongoDBConfig(
    MongoDBOrganizationId OrganizationId,
    MongoDBPublicKey PublicKey,
    MongoDBPrivateKey PrivateKey,
    MongoDBPassword KernelUserPassword);
