// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments;

namespace Read.Applications.Environments;

public class EnvironmentVariablesForApplicationEnvironmentProjection : IProjectionFor<EnvironmentVariablesForApplicationEnvironment>
{
    public ProjectionId Identifier => "09d24df9-92d6-4635-9648-2c0d12d47cc7";

    public void Define(IProjectionBuilderFor<EnvironmentVariablesForApplicationEnvironment> builder) => builder
        .Children(m => m.Variables, _ => _
            .IdentifiedBy(m => m.Key)
            .From<EnvironmentVariableSetForApplicationEnvironment>(_ => _
                .UsingParentKey(e => e.EnvironmentId)
                .UsingKey(e => e.Key)
                .Set(m => m.Value).To(e => e.Value)));
}
