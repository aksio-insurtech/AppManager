// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Reactions.Applications;

public record Deployable(DeployableId Id, MicroserviceId MicroserviceId, DeployableName Name, string Image, IEnumerable<int> Ports, ConfigPath ConfigPath);
