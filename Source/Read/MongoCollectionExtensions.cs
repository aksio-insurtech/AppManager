// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Read;

public static class MongoCollectionExtensions
{
    public static IFindFluent<T, T> FindById<T>(this IMongoCollection<T> collection, object id) =>
        collection.Find(Builders<T>.Filter.Eq(new StringFieldDefinition<T, string>("_id"), id.ToString()!));

    public static async Task<ClientObservable<T>> ObserveId<T>(this IMongoCollection<T> collection, object id)
    {
        var filter = Builders<T>.Filter.Eq(new StringFieldDefinition<T, string>("_id"), id.ToString()!);
        var observable = new ClientObservable<T>();
        var response = await collection.FindAsync(filter);
        var result = response.FirstOrDefault();
        if (result is not null)
        {
            observable.OnNext(result);
        }
        var cursor = collection.Watch();

        _ = Task.Run(async () =>
        {
            try
            {
                while (await cursor.MoveNextAsync())
                {
                    if (!cursor.Current.Any()) continue;
                    var response = await collection.FindAsync(filter);
                    var result = response.FirstOrDefault();
                    if (result is not null)
                    {
                        observable.OnNext(result);
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Cursor disposed.");
            }
        });

        observable.ClientDisconnected = () => cursor.Dispose();

        return observable;
    }
}
