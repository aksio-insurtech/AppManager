// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;

namespace Reactions.Applications;

public class DeployableProjection : IImmediateProjectionFor<Deployable>
{
    public ProjectionId Identifier => "76f8ddd1-2bef-4e98-98d0-cca6b0e703c9";

    public void Define(IProjectionBuilderFor<Deployable> builder) => builder
        .From<DeployableCreated>(_ => _
            .Set(m => m.MicroserviceId).To(e => e.MicroserviceId)
            .Set(m => m.Name).To(e => e.Name))
        .From<DeployableImageChanged>(_ => _
            .Set(m => m.Image).To(e => e.ImageName));
}
