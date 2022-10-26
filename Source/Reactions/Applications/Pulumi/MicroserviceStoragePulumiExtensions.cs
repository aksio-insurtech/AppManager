// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using FileShare = Pulumi.AzureNative.Storage.FileShare;

namespace Reactions.Applications.Pulumi;

public static class MicroserviceStoragePulumiExtensions
{
    public static async Task<MicroserviceStorage> GetStorage(this Microservice microservice, Application application, ApplicationEnvironmentWithArtifacts environment, ResourceGroup resourceGroup, ILogger<FileStorage> fileStorageLogger)
    {
        var getStorageAccountResult = GetStorageAccount.Invoke(new()
        {
            AccountName = environment.Resources.AzureStorageAccountName.Value,
            ResourceGroupName = resourceGroup.Name
        });
        var storageAccount = await getStorageAccountResult.GetValue();

        var storageAccountKeysRequest = ListStorageAccountKeys.Invoke(new ListStorageAccountKeysInvokeArgs
        {
            AccountName = storageAccount.Name,
            ResourceGroupName = resourceGroup.Name
        });

        var fileShare = new FileShare(microservice.Name.Value.ToLowerInvariant(), new()
        {
            AccountName = storageAccount.Name,
            ResourceGroupName = resourceGroup.Name
        });

        var fileShareName = await fileShare.Name.GetValue();
        var storageAccountKey = await storageAccountKeysRequest.GetValue(_ => _.Keys[0].Value);
        var fileStorage = new FileStorage(storageAccount.Name, storageAccountKey, fileShareName, fileStorageLogger);
        return new MicroserviceStorage(application, microservice, fileStorage);
    }
}
