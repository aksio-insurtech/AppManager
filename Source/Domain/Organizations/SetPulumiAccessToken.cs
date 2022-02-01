// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Organizations;
using ExecutionContext = Aksio.Cratis.Execution.ExecutionContext;

namespace Domain.Organizations
{
    public record SetPulumiAccessToken(string AccessToken);
}
