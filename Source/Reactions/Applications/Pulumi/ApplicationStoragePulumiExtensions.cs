// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using FileShare = Pulumi.AzureNative.Storage.FileShare;

namespace Reactions.Applications.Pulumi;

public static class ApplicationStoragePulumiExtensions
{
    public static async Task<StorageResult> SetupStorage(this Application application, ResourceGroup resourceGroup, Tags tags)
    {
        var storageAccount = new StorageAccount(application.Name.Value.ToLowerInvariant(), new StorageAccountArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            Sku = new SkuArgs
            {
                Name = SkuName.Standard_LRS
            },
            Kind = Kind.StorageV2
        });

        var fileShare = new FileShare("kernel", new()
        {
            AccountName = storageAccount.Name,
            ResourceGroupName = resourceGroup.Name,
        });

        var storageAccountKeysRequest = ListStorageAccountKeys.Invoke(new ListStorageAccountKeysInvokeArgs
        {
            AccountName = storageAccount.Name,
            ResourceGroupName = resourceGroup.Name
        });

        var fileShareName = await fileShare.Name.GetValue();
        var storageAccountName = await storageAccount.Name.GetValue();
        var storageAccountKey = await storageAccountKeysRequest.GetValue(_ => _.Keys[0].Value);

        return new(storageAccount, fileShareName, storageAccountName, storageAccountKey);
    }
}
