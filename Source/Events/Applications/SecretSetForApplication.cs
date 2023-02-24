// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Events.Applications;

[EventType("87123171-72f1-4d86-bc48-c3a9f93382d3")]
public record SecretSetForApplication(SecretKey Key, SecretValue Value);
