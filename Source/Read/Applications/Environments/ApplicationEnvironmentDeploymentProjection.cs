// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments;
using Events.Applications.Environments.Microservices.Modules.Deployables;

namespace Read.Applications.Environments;

public class ApplicationEnvironmentDeploymentProjection : IProjectionFor<ApplicationEnvironmentDeployment>
{
    public ProjectionId Identifier => "ede22f24-e6d8-491b-bc46-09e9112f4990";

    public void Define(IProjectionBuilderFor<ApplicationEnvironmentDeployment> builder) => builder
            .From<ApplicationEnvironmentDeploymentStarted>(_ => _
                .UsingCompositeKey<ApplicationEnvironmentDeploymentKey>(key => key
                    .Set(k => k.ApplicationId).To(e => e.ApplicationId)
                    .Set(k => k.EnvironmentId).To(e => e.EnvironmentId)
                    .Set(k => k.DeploymentId).To(e => e.DeploymentId))
                .Set(m => m.Status).ToValue(ApplicationEnvironmentDeploymentStatus.InProgress)
                .Set(m => m.Started).ToEventContextProperty(c => c.Occurred))
            .From<ApplicationEnvironmentDeploymentFailed>(_ => _
                .UsingCompositeKey<ApplicationEnvironmentDeploymentKey>(key => key
                    .Set(k => k.ApplicationId).To(e => e.ApplicationId)
                    .Set(k => k.EnvironmentId).To(e => e.EnvironmentId)
                    .Set(k => k.DeploymentId).To(e => e.DeploymentId))
                .Set(m => m.Status).ToValue(ApplicationEnvironmentDeploymentStatus.Failed)
                .Set(m => m.CompletedOrFailed).ToEventContextProperty(c => c.Occurred)
                .Set(m => m.Errors).To(e => e.Errors)
                .Set(m => m.StackTrace).To(e => e.StackTrace))
            .From<ApplicationEnvironmentDeploymentCompleted>(_ => _
                .UsingCompositeKey<ApplicationEnvironmentDeploymentKey>(key => key
                    .Set(k => k.ApplicationId).To(e => e.ApplicationId)
                    .Set(k => k.EnvironmentId).To(e => e.EnvironmentId)
                    .Set(k => k.DeploymentId).To(e => e.DeploymentId))
                .Set(m => m.Status).ToValue(ApplicationEnvironmentDeploymentStatus.Completed)
                .Set(m => m.CompletedOrFailed).ToEventContextProperty(c => c.Occurred))
            .From<DeployableImageChangedInEnvironment>(_ => _
                .UsingCompositeKey<ApplicationEnvironmentDeploymentKey>(key => key
                    .Set(k => k.ApplicationId).To(e => e.ApplicationId)
                    .Set(k => k.EnvironmentId).To(e => e.EnvironmentId)
                    .Set(k => k.DeploymentId).To(e => e.DeploymentId))
                .Set(m => m.Status).ToValue(ApplicationEnvironmentDeploymentStatus.InProgress)
                .Set(m => m.Started).ToEventContextProperty(c => c.Occurred));
}
