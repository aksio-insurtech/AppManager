// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Events.Applications;

[EventType("d933a813-69cc-4605-a2ae-5527cdc43144")]
public record ConfigFileSetForApplication(ConfigFileName Name, ConfigFileContent Content);
