// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts;
using Concepts.Applications;
using Concepts.Resources;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Pulumi.AzureNative.App;
using Reactions.Applications.Pulumi.Resources.MongoDB;

namespace Reactions.Applications.Pulumi.Resources.Cratis;

public class CratisResourceRenderer : ICanRenderResource<CratisConfiguration>
{
    readonly DockerHub _dockerHub;
    readonly ILogger<FileStorage> _fileStorageLogger;

    public ResourceLevel Level => ResourceLevel.Environment;
    public ResourceTypeId TypeId => "bf7864ab-b28f-4968-882a-3dc7477b9d0c";
    public IEnumerable<ResourceTypeId> Dependencies => Enumerable.Empty<ResourceTypeId>();

    public CratisResourceRenderer(DockerHub dockerHub, ILogger<FileStorage> fileStorageLogger)
    {
        _dockerHub = dockerHub;
        _fileStorageLogger = fileStorageLogger;
    }

    public async Task Render(RenderContextForResource context, CratisConfiguration configuration)
    {
        var cratisVersion = configuration.Version;
        if (configuration.Version == SemanticVersion.NotSet)
        {
            cratisVersion = await _dockerHub.GetLastVersionOfCratis();
        }

        var microservice = new Microservice(
            Guid.Empty,
            "kernel",
            AppSettingsContent.Empty,
            new Deployable[]
            {
                new Deployable(Guid.Empty, Guid.Empty, "kernel", $"docker.io/aksioinsurtech/cratis:{cratisVersion}", new[] { 80 }, MountPath.Default)
            },
            Enumerable.Empty<MicroserviceId>());

        var storage = context.Results.GetById<StorageResult>(WellKnownResourceTypes.Storage);
        var managedEnvironment = context.Results.GetById<ManagedEnvironment>(WellKnownResourceTypes.ManagedEnvironment);
        var applicationMonitoring = context.Results.GetById<ApplicationMonitoringResult>(WellKnownResourceTypes.ApplicationMonitoring);
        var mongoDB = context.Results.GetById<MongoDBResult>(MongoDBResourceRenderer.ResourceTypeId);

        var fileStorage = new FileStorage(storage.AccountName, storage.AccountKey, "kernel", _fileStorageLogger);

        var kernelStorage = new MicroserviceStorage(context.Application, microservice, fileStorage);

        var kernel = await microservice.SetupContainerApp(
            context.Application,
            context.ResourceGroup,
            managedEnvironment,
            null,
            kernelStorage,
            microservice.Deployables,
            context.Tags);

        await kernelStorage.CreateAndUploadCratisJson(
            mongoDB,
            context.Tenants,
            context.Microservices,
            kernel.SiloHostName,
            fileStorage.ConnectionString,
            applicationMonitoring);
    }
}
