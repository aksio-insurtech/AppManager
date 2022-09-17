// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Concepts.Applications;

namespace Events.Applications;

[EventType("80aff3f2-0bed-4740-8de0-4f524e7fb9d9")]
public record ApplicationFrontendConfigured(MicroserviceId MicroserviceId, CloudRuntimeEnvironment Environment);
