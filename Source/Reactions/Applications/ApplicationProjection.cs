// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;

namespace Reactions.Applications;

public class ApplicationProjection : IImmediateProjectionFor<Application>
{
    public ProjectionId Identifier => "a98f7e78-cc46-403b-84d0-5487206f1c70";

    public void Define(IProjectionBuilderFor<Application> builder) => builder
        .From<ApplicationCreated>(_ => _
            .Set(m => m.Id).ToEventSourceId()
            .Set(m => m.Name).To(e => e.Name)
            .Set(m => m.Shared.AzureSubscriptionId).To(e => e.SharedAzureSubscriptionId));
}
