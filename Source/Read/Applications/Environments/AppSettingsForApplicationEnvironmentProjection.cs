// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments;

namespace Read.Applications.Environments;

public class AppSettingsForApplicationEnvironmentProjection : IProjectionFor<AppSettingsForApplicationEnvironment>
{
    public ProjectionId Identifier => "661c994c-485e-4661-a206-da7c225d858c";

    public void Define(IProjectionBuilderFor<AppSettingsForApplicationEnvironment> builder) => builder
        .From<AppSettingsSetForApplicationEnvironment>(_ => _
            .Set(m => m.Content).To(e => e.Content));
}
