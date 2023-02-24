// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments;

namespace Read.Applications.Environments;

public class ConfigFilesForApplicationEnvironmentProjection : IProjectionFor<ConfigFilesForApplicationEnvironment>
{
    public ProjectionId Identifier => "d81fb43a-4650-418c-b212-97efbffe3b3c";

    public void Define(IProjectionBuilderFor<ConfigFilesForApplicationEnvironment> builder) => builder
        .Children(m => m.Files, _ => _
            .IdentifiedBy(m => m.Name)
            .From<ConfigFileSetForApplicationEnvironment>(_ => _
                .UsingKey(e => e.Name)
                .Set(m => m.Content).To(e => e.Content)));
}
