// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Domain.Applications.Environments.Microservices.Modules;

public record CreateModule(ModuleId ModuleId, ModuleName Name);
