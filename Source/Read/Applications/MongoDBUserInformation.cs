// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.MongoDB;

namespace Reactions.Applications
{
    public record MongoDBUserInformation(MongoDBUser User, MongoDBPassword Password);
}
