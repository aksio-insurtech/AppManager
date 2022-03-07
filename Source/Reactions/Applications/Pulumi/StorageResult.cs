// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.Storage;

namespace Reactions.Applications.Pulumi;

public record StorageResult(StorageAccount Account, string FileShare, string AccountName, string AccountKey);
