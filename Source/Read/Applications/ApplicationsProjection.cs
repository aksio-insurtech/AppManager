// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;

namespace Read.Applications;

public class ApplicationsProjection : IProjectionFor<Application>
{
    public ProjectionId Identifier => "04de97aa-3d87-4464-8add-eb703df0d42c";

    public void Define(IProjectionBuilderFor<Application> builder) => builder
        .From<ApplicationCreated>(_ => _
            .Set(m => m.Name).To(e => e.Name));
}
