// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;

namespace Read.Applications;

public class ConfigFilesForApplicationProjection : IProjectionFor<ConfigFilesForApplication>
{
    public ProjectionId Identifier => "e77d0e46-ba4f-4053-8211-44f482266fe6";

    public void Define(IProjectionBuilderFor<ConfigFilesForApplication> builder) => builder
        .Children(m => m.Files, _ => _
            .IdentifiedBy(m => m.Name)
            .From<ConfigFileSetForApplication>(_ => _
                .UsingKey(e => e.Name)
                .Set(m => m.Content).To(e => e.Content)));
}
