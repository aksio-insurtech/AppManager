// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Reactions.Applications;

public record Microservice(
    MicroserviceId Id,
    MicroserviceName Name,
    AppSettingsContent AppSettingsContent,
    IEnumerable<Module> Modules,
    IEnumerable<MicroserviceId> ConnectedWith)
{
    public Module GetModuleById(ModuleId id) => Modules.Single(_ => _.Id == id);
}
