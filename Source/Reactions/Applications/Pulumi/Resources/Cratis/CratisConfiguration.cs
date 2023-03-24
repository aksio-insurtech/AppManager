// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Serialization;
using Concepts;

namespace Reactions.Applications.Pulumi.Resources.Cratis;

[DerivedType("389882c8-f534-4a97-8ed0-ed584286f7ee")]
public record CratisConfiguration(SemanticVersion Version) : IResourceConfiguration;
