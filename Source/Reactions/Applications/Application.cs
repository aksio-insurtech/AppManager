// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.Organizations;
using ApplicationId = Concepts.Applications.ApplicationId;

namespace Reactions.Applications
{
    public record Application(ApplicationId Id, ApplicationName Name, AzureSubscriptionId AzureSubscriptionId, string PulumiAccessToken, CloudLocationKey CloudLocation);
}
