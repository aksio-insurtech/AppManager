// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Common;
using Concepts.Pulumi;

namespace Read.Organizations
{
    public class ApplicationSettings : IApplicationSettings
    {
        readonly ExecutionContext _executionContext;
        readonly IMongoCollection<Settings> _settingsCollection;

        public ApplicationSettings(ExecutionContext executionContext, IMongoCollection<Settings> settingsCollection)
        {
            _executionContext = executionContext;
            _settingsCollection = settingsCollection;
        }

        public async Task<PulumiAccessToken> GetPulumiAccessToken()
        {
            var settings = await GetSettings();
            return settings.PulumiAccessToken;
        }

        Task<Settings> GetSettings() => _settingsCollection.Find(_ => _.Id == _executionContext.TenantId).FirstAsync();
    }
}
