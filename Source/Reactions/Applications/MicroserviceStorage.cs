// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Encodings.Web;
using System.Text.Json;
using Aksio.Cratis.Configuration;
using Aksio.Cratis.Extensions.Orleans.Configuration;
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

    public async Task CreateAndUploadCratisJson(MongoDBResult mongoDB, string siloHostName, string storageConnectionString, ApplicationMonitoringResult monitoring)
    {
        const string scheme = "mongodb+srv://";
        var mongoDBConnectionString = mongoDB.ConnectionString.Insert(scheme.Length, $"kernel:{mongoDB.Password}@");

        var appInsightsInstrumentationKey = await monitoring.ApplicationInsight.InstrumentationKey.GetValue();

        var config = new KernelConfiguration()
        {
            Cluster = new()
            {
                Name = "Cratis",
                Type = ClusterTypes.AzureStorage,
                AdvertisedIP = string.Empty,
                SiloPort = 11111,
                GatewayPort = 30000,
                Options = new AzureStorageClusterOptions
                {
                    ConnectionString = storageConnectionString,
                    TableName = "OrleansSiloInstances"
                }
            },
            Telemetry = new()
            {
                Type = TelemetryTypes.AppInsights,
                Options = new AppInsightsTelemetryOptions
                {
                    Key = appInsightsInstrumentationKey
                }
            },
            Storage = new()
            {
                Cluster = new()
                {
                    Type = "MongoDB",
                    ConnectionDetails = $"{mongoDBConnectionString}/cratis-shared"
                }
            }
        };
        config.Tenants["3352d47d-c154-4457-b3fb-8a2efb725113"] = new() { Name = "Development" };
        config.Microservices["00000000-0000-0000-0000-000000000000"] = new() { Name = "Something" };
        config.Storage.Microservices["00000000-0000-0000-0000-000000000000"] = new()
        {
            Shared = new()
            {
                {
                    "eventStore", new()
                    {
                        Type = "MongoDB",
                        ConnectionDetails = $"{mongoDBConnectionString}/event-store-shared"
                    }
                }
            },
            Tenants = new()
            {
                {
                    "3352d47d-c154-4457-b3fb-8a2efb725113", new()
                    {
                        {
                            "readModels", new()
                            {
                                Type = "MongoDB",
                                ConnectionDetails = $"{mongoDBConnectionString}/development-read-models"
                            }
                        },
                        {
                            "eventStore", new()
                            {
                                Type = "MongoDB",
                                ConnectionDetails = $"{mongoDBConnectionString}/development-event-store"
                            }
                        }
                    }
                }
            }
        };

        var cratisJson = JsonSerializer.Serialize(config, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        FileStorage.Upload("cratis.json", cratisJson);
    }

    public void CreateAndUploadAppSettings(ISettings settings)
    {
        var content = TemplateTypes.AppSettings(
            new AppSettingsValues(_application.Name, _microservice.Name));
        FileStorage.Upload("appsettings.json", content);
    }

    public void CreateAndUploadClusterClientConfig(string connectionString)
    {
        var content = TemplateTypes.ClusterClient(new { ConnectionString = connectionString });
        FileStorage.Upload("cluster.json", content);
    }
}
