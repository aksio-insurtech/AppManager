// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Read.Organizations
{
    [Route("/api/organization/settings")]
    public class OrganizationSettings : Controller
    {
        readonly IMongoCollection<Settings> _collection;

        public OrganizationSettings(IMongoCollection<Settings> collection)
        {
            _collection = collection;
        }

        [HttpGet]
        public async Task<Settings> AllSettings() => await _collection.Find(_ => true).FirstOrDefaultAsync() ?? Settings.NoSettings;
    }
}
