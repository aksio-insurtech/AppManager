// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Encodings.Web;
using System.Text.Json;
using Common;
using Reactions.Applications.Pulumi;
using Reactions.Applications.Templates;

namespace Reactions.Applications;

public class MicroserviceStorage
{
    readonly Application _application;
    readonly Microservice _microservice;

    public FileStorage FileStorage { get; }

    public MicroserviceStorage(
        Application application,
        Microservice microservice,
        FileStorage fileStorage)
    {
        _application = application;
        _microservice = microservice;
        FileStorage = fileStorage;
    }

    public void CreateAndUploadStorageJson(MongoDBResult mongoDB)
    {
        const string scheme = "mongodb+srv://";
        var mongoDBConnectionString = mongoDB.ConnectionString.Insert(scheme.Length, $"kernel:{mongoDB.Password}@");

        var storageConfig = new Aksio.Cratis.Configuration.Storage();
        var readModels = storageConfig["readModels"] = new()
        {
            Type = "MongoDB",
            Shared = null!
        };
        readModels.Tenants["3352d47d-c154-4457-b3fb-8a2efb725113"] = $"{mongoDBConnectionString}/read-models";

        var eventStore = storageConfig["eventStore"] = new()
        {
            Type = "MongoDB",
            Shared = $"{mongoDBConnectionString}/event-store-shared"
        };
        eventStore.Tenants["3352d47d-c154-4457-b3fb-8a2efb725113"] = $"{mongoDBConnectionString}/event-store";

        var storageJson = JsonSerializer.Serialize(storageConfig, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        FileStorage.Upload("storage.json", storageJson);
    }

    public void CreateAndUploadAppSettings(ISettings settings)
    {
        var content = TemplateTypes.AppSettings(
            new AppSettingsValues(_application.Name, _microservice.Name, settings.ElasticUrl, settings.ElasticApiKey));
        FileStorage.Upload("appsettings.json", content);
    }

    public void CreateAndUploadClusterClientConfig(string connectionString)
    {
        var content = TemplateTypes.ClusterClient(new { ConnectionString = connectionString });
        FileStorage.Upload("cluster.json", content);
    }

    public void CreateAndUploadClusterKernelConfig(string siloHostName, string connectionString)
    {
        var content = TemplateTypes.ClusterKernel(new { SiloHostName = siloHostName, ConnectionString = connectionString });
        FileStorage.Upload("cluster.json", content);
    }
}
