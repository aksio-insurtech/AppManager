// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Organizations;

namespace Read.Organizations
{
    public class SettingsProjection : IProjectionFor<Settings>
    {
        public ProjectionId Identifier => "5398c7f5-2baf-494b-abac-0a35eeb5fd98";

        public void Define(IProjectionBuilderFor<Settings> builder) => builder
            .From<PulumiAccessTokenSet>(_ => _
                .Set(m => m.PulumiAccessToken).To(e => e.AccessToken))
            .From<MongoDBSettingsSet>(_ => _
                .Set(m => m.MongoDBOrganizationId).To(e => e.OrganizationId)
                .Set(m => m.MongoDBPublicKey).To(e => e.PublicKey)
                .Set(m => m.MongoDBPrivateKey).To(e => e.PrivateKey))
            .From<ElasticSearchSettingsSet>(_ => _
                .Set(m => m.ElasticUrl).To(e => e.Url)
                .Set(m => m.ElasticApiKey).To(e => e.ApiKey))
            .Children(_ => _.AzureSubscriptions, _ => _
                .IdentifiedBy(s => s.SubscriptionId)
                .From<AzureSubscriptionAdded>(f => f
                    .UsingKey(_ => _.Id)
                    .Set(m => m.Name).To(e => e.Name)
                    .Set(m => m.TenantName).To(e => e.TenantName)));
    }
}
