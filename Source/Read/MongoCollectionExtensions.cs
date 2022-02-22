// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Read;

public static class MongoCollectionExtensions
{
    public static IFindFluent<T, T> FindById<T>(this IMongoCollection<T> collection, object id) =>
        collection.Find(Builders<T>.Filter.Eq(new StringFieldDefinition<T, string>("_id"), id.ToString()!));
}
