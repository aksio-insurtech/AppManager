// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Authorization;

namespace Events.Applications.Authorization;

[EventType("1e71a4d2-a867-4d35-90f0-6f314cb6d993")]
public record UserAllowedAccessToApplication(UserName UserName);
