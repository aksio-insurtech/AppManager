// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using FileShare = Pulumi.AzureNative.Storage.FileShare;

namespace Reactions.Applications.Pulumi;

#pragma warning disable RCS1175

public static class ApplicationStoragePulumiExtensions
{
    public static async Task<StorageResult> SetupStorage(this Application application, ApplicationEnvironment environment, ResourceGroup resourceGroup, Tags tags)
    {
        var truncatedName = application.Name.Value;
        if (truncatedName.Length > 15)
        {
            truncatedName = truncatedName.Substring(0, 15);
        }

        var name = $"{environment.ShortName}{truncatedName}".ToLowerInvariant();
        var storageAccount = new StorageAccount(name, new StorageAccountArgs
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

    public static async Task<Storage> GetStorage(this Application application, ApplicationEnvironmentWithArtifacts environment, ResourceGroup resourceGroup)
    {
        var getStorageAccountResult = GetStorageAccount.Invoke(new()
        {
            AccountName = environment.ApplicationResources.AzureStorageAccountName.Value,
            ResourceGroupName = resourceGroup.Name
        });
        var storageAccount = await getStorageAccountResult.GetValue();

        var storageAccountKeysRequest = ListStorageAccountKeys.Invoke(new ListStorageAccountKeysInvokeArgs
        {
            AccountName = storageAccount.Name,
            ResourceGroupName = resourceGroup.Name
        });

        var storageAccountKey = await storageAccountKeysRequest.GetValue(_ => _.Keys[0].Value);
        return new Storage(storageAccount.Name, storageAccountKey);
    }
}
