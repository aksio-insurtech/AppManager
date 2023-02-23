// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Concepts.Azure;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Logging;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using FileShare = Pulumi.AzureNative.Storage.FileShare;

namespace Reactions.Applications.Pulumi;

#pragma warning disable RCS1175

public static class StoragePulumiExtensions
{
    public static async Task<StorageResult> SetupStorage(
        this Application application,
        ApplicationEnvironment environment,
        ResourceGroup resourceGroup,
        Tags tags)
    {
        var name = application.GetStorageAccountName(environment);
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

    public static async Task<Storage> GetStorage(
        this Application application,
        ApplicationEnvironmentWithArtifacts environment,
        ResourceGroup resourceGroup,
        AzureServicePrincipal servicePrincipal,
        AzureSubscription subscription)
    {
        var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(
            clientId: servicePrincipal.ClientId,
            clientSecret: servicePrincipal.ClientSecret,
            tenantId: subscription.TenantId,
            environment: AzureEnvironment.AzureGlobalCloud);

        var azure = Microsoft.Azure.Management.Fluent.Azure
            .Configure()
            .Authenticate(credentials)
            .WithSubscription(subscription.SubscriptionId);

        var resourceGroupName = application.GetResourceGroupName(environment, environment.CloudLocation);
        var storageAccounts = await azure.StorageAccounts.ListByResourceGroupAsync(resourceGroupName);
        var storageAccountName = application.GetStorageAccountName(environment);
        var storageAccount = storageAccounts.FirstOrDefault(_ => _.Name.StartsWith(storageAccountName));
        var keys = await storageAccount!.GetKeysAsync();
        return new Storage(storageAccount.Name, keys[0].Value);
    }

    public static async Task<MicroserviceStorage> GetStorage(
        this Microservice microservice,
        Application application,
        ApplicationEnvironmentWithArtifacts environment,
        ResourceGroup resourceGroup,
        AzureServicePrincipal servicePrincipal,
        AzureSubscription subscription,
        ILogger<FileStorage> fileStorageLogger)
    {
        var storage = await application.GetStorage(environment, resourceGroup, servicePrincipal, subscription);

        var fileShareName = microservice.Name.Value.ToLowerInvariant();
        var fileShare = GetFileShare.Invoke(new()
        {
            AccountName = storage.AccountName,
            ShareName = fileShareName,
            ResourceGroupName = resourceGroup.Name
        });
        var name = fileShare.Apply(_ => _.Name);
        fileShareName = await name.GetValue();
        var fileStorage = new FileStorage(storage.AccountName, storage.AccountKey, fileShareName, fileStorageLogger);
        return new MicroserviceStorage(application, microservice, fileStorage);
    }

    public static string GetStorageAccountName(this Application application, ApplicationEnvironment environment)
    {
        var truncatedName = application.Name.Value;
        if (truncatedName.Length > 15)
        {
            truncatedName = truncatedName.Substring(0, 15);
        }

        return $"{environment.ShortName}{truncatedName}".ToLowerInvariant();
    }
}
