// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments;

namespace Read.Applications.Environments;

public record ApplicationEnvironmentProjection : IProjectionFor<ApplicationEnvironment>
{
    public ProjectionId Identifier => "27449588-0f8c-47e4-b1ee-c326a12ed391";

    public void Define(IProjectionBuilderFor<ApplicationEnvironment> builder) => builder
        .From<ApplicationEnvironmentCreated>(_ => _
            .UsingCompositeKey<ApplicationEnvironmentKey>(_ => _
                .Set(k => k.ApplicationId).To(e => e.ApplicationId)
                .Set(k => k.EnvironmentId).ToEventSourceId())
            .Set(m => m.Name).To(e => e.Name)
            .Set(m => m.DisplayName).To(e => e.DisplayName)
            .Set(m => m.ShortName).To(e => e.ShortName));
}
