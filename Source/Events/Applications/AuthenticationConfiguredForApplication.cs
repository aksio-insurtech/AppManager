// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Azure;

namespace Events.Applications;

[EventType("b20af576-48a4-4293-8859-9e2c7ed5ebc7")]
public record AuthenticationConfiguredForApplication(ClientId ClientId, ClientSecret ClientSecret);
