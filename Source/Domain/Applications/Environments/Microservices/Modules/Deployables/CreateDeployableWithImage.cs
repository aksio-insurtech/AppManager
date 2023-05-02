// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Domain.Applications.Environments.Microservices.Modules.Deployables;

public record CreateDeployableWithImage(DeployableId DeployableId, DeployableName Name, DeployableImageName Image);
