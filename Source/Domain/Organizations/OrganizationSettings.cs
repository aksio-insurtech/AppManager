// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Organizations;
using Concepts.Pulumi;
using Events.Organizations;

namespace Domain.Organizations
{
    [Route("/api/organization/settings")]
    public class OrganizationSettings : Controller
    {
        readonly IEventLog _eventLog;

        public OrganizationSettings(IEventLog eventLog)
        {
            _eventLog = eventLog;
        }

        [HttpPost("subscriptions")]
        public Task AddAzureSubscription([FromBody] AddAzureSubscription command) => _eventLog.Append(SettingsId.Global, new AzureSubscriptionAdded(command.Id, command.Name));

        [HttpPost("pulumi")]
        public Task SetPulumiAccessToken([FromBody] PulumiAccessToken accessToken) => _eventLog.Append(SettingsId.Global, new PulumiAccessTokenSet(accessToken));

        [HttpPost("mongodb")]
        public Task SetMongoDBSettings([FromBody] MongoDBSettings settings) => _eventLog.Append(SettingsId.Global, new MongoDBSettingsSet(settings.OrganizationId, settings.PublicKey, settings.PrivateKey));
    }
}
