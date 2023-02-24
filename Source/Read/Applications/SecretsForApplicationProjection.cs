// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Events.Applications;

namespace Read.Applications;

public class SecretsForApplicationProjection : IProjectionFor<SecretsForApplication>
{
    public ProjectionId Identifier => "9887f8ba-a677-4495-8ea1-1aa8091506b9";

    public void Define(IProjectionBuilderFor<SecretsForApplication> builder) => builder
        .Children(m => m.Secrets, _ => _
            .IdentifiedBy(m => m.Key)
            .From<SecretSetForApplication>(_ => _
                .UsingKey(e => e.Key)
                .Set(m => m.Value).To(e => e.Value)));
}
