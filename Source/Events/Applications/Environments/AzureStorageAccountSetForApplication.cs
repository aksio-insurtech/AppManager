// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Azure;

namespace Events.Applications.Environments;

[EventType("df24d5c8-b4dd-4ff5-8636-588ec6699177")]
public record AzureStorageAccountSetForApplication(AzureStorageAccountName AccountName);
