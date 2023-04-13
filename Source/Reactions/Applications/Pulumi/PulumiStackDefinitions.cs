// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Common;
using Concepts.Applications.Environments;
using Microsoft.Extensions.Logging;
using Pulumi;
using Pulumi.AzureNative.App;
using Pulumi.AzureNative.Resources;
using Reactions.Applications.Pulumi.Resources;
using Reactions.Applications.Templates;

namespace Reactions.Applications.Pulumi;

public class PulumiStackDefinitions : IPulumiStackDefinitions
{
    public static readonly ApplicationEnvironment SharedEnvironment = new(
        Guid.Parse("5e4d2c45-4ebb-4bc4-a178-b1d1d5ed44ff"),
        "Shared",
        "shared",
        "S");

    readonly ISettings _settings;
    readonly IExecutionContextManager _executionContextManager;
    readonly ILogger<FileStorage> _fileStorageLogger;

    public PulumiStackDefinitions(
        ISettings applicationSettings,
        IExecutionContextManager executionContextManager,
        ILogger<FileStorage> fileStorageLogger)
    {
        _settings = applicationSettings;
        _executionContextManager = executionContextManager;
        _fileStorageLogger = fileStorageLogger;
    }

    public Task<ResourceGroup> Application(
        Application application,
        ApplicationEnvironmentWithArtifacts sharedEnvironment,
        ResourceResultsByType resourceResults)
    {
        var sharedResourceGroup = application.SetupResourceGroup(sharedEnvironment);

        return Task.FromResult(sharedResourceGroup);
    }

    public async Task<ApplicationEnvironmentResult> ApplicationEnvironment(
        ExecutionContext executionContext,
        Application application,
        ApplicationEnvironmentWithArtifacts environment,
        ApplicationEnvironmentWithArtifacts sharedEnvironment,
        ResourceResultsByType resourceResults)
    {
        var sharedSubscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == sharedEnvironment.AzureSubscriptionId);
        var containerRegistry = await application.GetContainerRegistry(sharedEnvironment, _settings.ServicePrincipal, sharedSubscription);
        if (containerRegistry is not null)
        {
            resourceResults.Register(WellKnownResourceTypes.ContainerRegistry, containerRegistry);
        }

        var tags = application.GetTags(environment);
        var subscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == environment.AzureSubscriptionId);
        var resourceGroup = application.SetupResourceGroup(environment);
        var identity = application.SetupUserAssignedIdentity(environment, resourceGroup, tags);
        var vault = application.SetupKeyVault(environment, identity, resourceGroup, tags);
        var network = application.SetupNetwork(environment, identity, vault, resourceGroup, tags);
        resourceResults.Register(WellKnownResourceTypes.VirtualNetwork, network.VirtualNetwork);

        var storage = await application.SetupStorage(environment, resourceGroup, tags);
        resourceResults.Register(WellKnownResourceTypes.Storage, storage);

        var fileStorage = new FileStorage(storage.AccountName, storage.AccountKey, storage.FileShare, _fileStorageLogger);
        var applicationMonitoring = application.SetupApplicationMonitoring(resourceGroup, environment, tags);
        resourceResults.Register(WellKnownResourceTypes.ApplicationMonitoring, applicationMonitoring);

        var managedEnvironment = application.SetupContainerAppManagedEnvironment(resourceGroup, environment, applicationMonitoring.Workspace, network, tags);
        resourceResults.Register(WellKnownResourceTypes.ManagedEnvironment, managedEnvironment);

        var certificates = application.SetupCertificates(environment, managedEnvironment, resourceGroup, tags);

        var content = TemplateTypes.AppSettings(null!);
        fileStorage.Upload("appsettings.json", content);

        return new ApplicationEnvironmentResult(
            environment,
            resourceGroup,
            network,
            storage,
            managedEnvironment,
            certificates);
    }

    public async Task<IngressResult> Ingress(
        ExecutionContext executionContext,
        Application application,
        ApplicationEnvironmentWithArtifacts environment,
        Ingress ingress,
        ManagedEnvironment managedEnvironment,
        IDictionary<CertificateId, Output<string>> certificates,
        ResourceGroup? resourceGroup = default)
    {
        var tags = application.GetTags(environment);
        resourceGroup ??= application.GetResourceGroup(environment);
        var subscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == environment.AzureSubscriptionId);
        var storage = await application.GetStorage(environment, _settings.ServicePrincipal, subscription);

        return await application.SetupIngress(
            environment,
            resourceGroup,
            storage,
            managedEnvironment,
            certificates,
            ingress,
            tags,
            _fileStorageLogger);
    }

    public async Task<ContainerAppResult> Microservice(
        ExecutionContext executionContext,
        Application application,
        Microservice microservice,
        ResourceGroup resourceGroup,
        ApplicationEnvironmentWithArtifacts environment,
        ManagedEnvironment managedEnvironment,
        ResourceResultsByType resourceResults,
        IEnumerable<Deployable>? deployables = default)
    {
        var tags = application.GetTags(environment);
        var subscription = _settings.AzureSubscriptions.First(_ => _.SubscriptionId == environment.AzureSubscriptionId);
        var storage = await microservice.SetupFileShare(application, environment, resourceGroup, _settings.ServicePrincipal, subscription, _fileStorageLogger);
        storage.CreateAndUploadAppSettings();

        deployables ??= Array.Empty<Deployable>();

        ContainerRegistryResult? containerRegistry = null;
        if (resourceResults.HasFor(WellKnownResourceTypes.ContainerRegistry))
        {
            containerRegistry = resourceResults.GetById<ContainerRegistryResult>(WellKnownResourceTypes.ContainerRegistry);
        }

        var microserviceResult = await microservice.SetupContainerApp(
            application,
            resourceGroup,
            managedEnvironment,
            containerRegistry,
            storage,
            deployables,
            tags);

        microserviceResult.ContainerApp.Configuration.Apply(_ => _?.Ingress).Apply(_ =>
        {
            var url = $"http://{_?.Fqdn}";
            storage.CreateAndUploadClientCratisConfig(storage.FileStorage.ConnectionString, url);
            return _?.Fqdn;
        });

        // Todo: Set to actual execution context - might not be the right place for this!
        _executionContextManager.Set(executionContext);

        return microserviceResult;
    }
}
