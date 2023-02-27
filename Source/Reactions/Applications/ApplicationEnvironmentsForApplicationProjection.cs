// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments;

namespace Reactions.Applications;

public class ApplicationEnvironmentsForApplicationProjection : IImmediateProjectionFor<ApplicationEnvironmentsForApplication>
{
    public ProjectionId Identifier => "60c18bbe-e2c7-41ec-a4ad-fea03b5941d1";

    public void Define(IProjectionBuilderFor<ApplicationEnvironmentsForApplication> builder) => builder
        .Children(m => m.Environments, cb => cb
            .IdentifiedBy(m => m.Id)
            .From<ApplicationEnvironmentAddedToApplication>(e => e
                .UsingKey(e => e.EnvironmentId)));
}
