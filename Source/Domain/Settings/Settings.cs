// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Azure;
using Concepts.Settings;
using Events.Settings;

namespace Domain.Settings;

[Route("/api/settings")]
public class Settings : Controller
{
    readonly IEventLog _eventLog;

    public Settings(IEventLog eventLog)
    {
        _eventLog = eventLog;
    }

    [HttpPost("service-principal")]
    public Task SetAzureServicePrincipal([FromBody] AzureServicePrincipal principal) => _eventLog.Append(SettingsId.Global, new AzureServicePrincipalSet(principal.ClientId, principal.ClientSecret));

    [HttpPost("subscriptions")]
    public Task AddAzureSubscription([FromBody] AzureSubscriptionToAdd subscription) => _eventLog.Append(SettingsId.Global, new AzureSubscriptionAdded(subscription.Id, subscription.Name, subscription.TenantId, subscription.TenantName));

    [HttpPost("pulumi")]
    public Task SetPulumiSettings([FromBody] PulumiSettings settings) => _eventLog.Append(SettingsId.Global, new PulumiSettingsSet(settings.Organization, settings.AccessToken));

    [HttpPost("mongodb")]
    public Task SetMongoDBSettings([FromBody] MongoDBSettings settings) => _eventLog.Append(SettingsId.Global, new MongoDBSettingsSet(settings.OrganizationId, settings.PublicKey, settings.PrivateKey));
}
