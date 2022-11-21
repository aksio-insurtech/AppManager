// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;

namespace Read.Applications;

public class EnvironmentVariablesForApplicationProjection : IProjectionFor<EnvironmentVariablesForApplication>
{
    public ProjectionId Identifier => "0dbb597b-0428-47e5-8451-f330524bca52";

    public void Define(IProjectionBuilderFor<EnvironmentVariablesForApplication> builder) => builder
        .Children(m => m.Variables, _ => _
            .IdentifiedBy(m => m.Key)
            .From<EnvironmentVariableSetForApplication>(_ => _
                .UsingKey(e => e.Key)
                .Set(m => m.Value).To(e => e.Value)));
}
