// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments;

namespace Read.Applications;

public class ApplicationEnvironmentConsolidationProjection : IProjectionFor<ApplicationEnvironmentConsolidation>
{
    public ProjectionId Identifier => "ede22f24-e6d8-491b-bc46-09e9112f4990";

    public void Define(IProjectionBuilderFor<ApplicationEnvironmentConsolidation> builder) => builder
            .From<ApplicationEnvironmentConsolidationStarted>(_ => _
                .UsingCompositeKey<ApplicationEnvironmentConsolidationKey>(key => key
                    .Set(k => k.ApplicationId).ToEventSourceId()
                    .Set(k => k.EnvironmentId).To(e => e.EnvironmentId)
                    .Set(k => k.ConsolidationId).To(e => e.ConsolidationId))
                .Set(m => m.Status).ToValue(ApplicationEnvironmentConsolidationStatus.InProgress)
                .Set(m => m.Started).ToEventContextProperty(c => c.Occurred))
            .From<ApplicationEnvironmentConsolidationFailed>(_ => _
                .UsingCompositeKey<ApplicationEnvironmentConsolidationKey>(key => key
                    .Set(k => k.ApplicationId).ToEventSourceId()
                    .Set(k => k.EnvironmentId).To(e => e.EnvironmentId)
                    .Set(k => k.ConsolidationId).To(e => e.ConsolidationId))
                .Set(m => m.Status).ToValue(ApplicationEnvironmentConsolidationStatus.Failed)
                .Set(m => m.CompletedOrFailed).ToEventContextProperty(c => c.Occurred))
            .From<ApplicationEnvironmentConsolidationCompleted>(_ => _
                .UsingCompositeKey<ApplicationEnvironmentConsolidationKey>(key => key
                    .Set(k => k.ApplicationId).ToEventSourceId()
                    .Set(k => k.EnvironmentId).To(e => e.EnvironmentId)
                    .Set(k => k.ConsolidationId).To(e => e.ConsolidationId))
                .Set(m => m.Status).ToValue(ApplicationEnvironmentConsolidationStatus.Completed)
                .Set(m => m.CompletedOrFailed).ToEventContextProperty(c => c.Occurred));
}
