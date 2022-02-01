// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Organizations;
using ExecutionContext = Aksio.Cratis.Execution.ExecutionContext;

namespace Domain.Organizations
{
    [Route("/api/organization/settings")]
    public class OrganizationSettings : Controller
    {
        readonly IEventLog _eventLog;
        readonly ExecutionContext _executionContext;

        public OrganizationSettings(IEventLog eventLog, ExecutionContext executionContext)
        {
            _eventLog = eventLog;
            _executionContext = executionContext;
        }

        [HttpPost("subscriptions")]
        public Task AddAzureSubscription([FromBody] AddAzureSubscription command) => _eventLog.Append(_executionContext.TenantId.ToString(), new AzureSubscriptionAdded(command.Id, command.Name));

        [HttpPost("pulumi")]
        public Task SetPulumiAccessToken([FromBody] SetPulumiAccessToken command) => _eventLog.Append(_executionContext.TenantId.ToString(), new PulumiAccessTokenSet(command.AccessToken));
    }
}
