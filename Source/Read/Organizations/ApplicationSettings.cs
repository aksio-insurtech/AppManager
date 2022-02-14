// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Common;
using Concepts.MongoDB;
using Concepts.Pulumi;

namespace Read.Organizations
{
    public class ApplicationSettings : IApplicationSettings
    {
        readonly IMongoCollection<Settings> _settingsCollection;

        public ApplicationSettings(IMongoCollection<Settings> settingsCollection)
        {
            _settingsCollection = settingsCollection;
        }

        public async Task<MongoDBOrganizationId> GetMongoDBOrganizationId()
        {
            var settings = await GetSettings();
            return settings.MongoDBOrganizationId;
        }

        public async Task<MongoDBPrivateKey> GetMongoDBPrivateKey()
        {
            var settings = await GetSettings();
            return settings.MongoDBPrivateKey;
        }

        public async Task<MongoDBPublicKey> GetMongoDBPublicKey()
        {
            var settings = await GetSettings();
            return settings.MongoDBPublicKey;
        }

        public async Task<PulumiAccessToken> GetPulumiAccessToken()
        {
            var settings = await GetSettings();
            return settings.PulumiAccessToken;
        }

        async Task<Settings> GetSettings() => await _settingsCollection.Find(_ => true).FirstOrDefaultAsync() ?? Settings.NoSettings;
    }
}
