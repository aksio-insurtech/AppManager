// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Azure;

namespace Events.Applications;

[EventType("a64c4d13-2ae4-4f2e-89f1-f85713d75fbe")]
public record AzureContainerRegistrySetForApplication(
    AzureContainerRegistryLoginServer LoginServer,
    AzureContainerRegistryUserName UserName,
    AzureContainerRegistryPassword Password);
