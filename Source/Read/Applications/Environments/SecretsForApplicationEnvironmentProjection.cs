// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications.Environments;

namespace Read.Applications.Environments;

public class SecretsForApplicationEnvironmentProjection : IProjectionFor<SecretsForApplicationEnvironment>
{
    public ProjectionId Identifier => "d9d4cba4-1497-4b95-b8e1-8b8d98f588d4";

    public void Define(IProjectionBuilderFor<SecretsForApplicationEnvironment> builder) => builder
        .Children(m => m.Secrets, _ => _
            .IdentifiedBy(m => m.Key)
            .From<SecretSetForApplicationEnvironment>(_ => _
                .UsingKey(e => e.Key)
                .Set(m => m.Value).To(e => e.Value)));
}
