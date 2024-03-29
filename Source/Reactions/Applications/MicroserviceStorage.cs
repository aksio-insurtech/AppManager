// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aksio.Cratis.Kernel.Configuration;
using Aksio.Cratis.Kernel.Orleans.Configuration;
using Reactions.Applications.Pulumi;

namespace Reactions.Applications;

public class MicroserviceStorage
{
    readonly Microservice _microservice;

    public FileStorage FileStorage { get; }

    public MicroserviceStorage(
        Application application,
        Microservice microservice,
        FileStorage fileStorage)
    {
        _microservice = microservice;
        FileStorage = fileStorage;
    }

    public async Task CreateAndUploadCratisJson(
        MongoDBResult mongoDB,
        IEnumerable<Tenant> tenants,
        IEnumerable<Microservice> microservices,
        string siloHostName,
        string storageConnectionString,
        ApplicationMonitoringResult monitoring)
    {
#pragma warning disable CA1851 // Possible multiple enumerations of IEnumerable
        const string scheme = "mongodb+srv://";
        var mongoDBConnectionString = mongoDB.ConnectionString.Insert(scheme.Length, $"kernel:{mongoDB.Password}@");

        var appInsightsInstrumentationKey = await monitoring.ApplicationInsight.InstrumentationKey.GetValue();
        var appInsightsConnectionString = await monitoring.ApplicationInsight.ConnectionString.GetValue();

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
                    Key = appInsightsInstrumentationKey,
                    ConnectionString = appInsightsConnectionString
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

        foreach (var tenant in tenants)
        {
            config.Tenants[tenant.Id.ToString()] = new() { Name = tenant.Name };
        }

        foreach (var microservice in microservices)
        {
            config.Microservices[microservice.Id.ToString()] = new()
            {
                Name = microservice.Name,
                Inbox = new()
                {
                    FromOutboxes = microservice.ConnectedWith is null ?
                        new Collection<Outbox>() :
                        new Collection<Outbox>(microservice.ConnectedWith.Select(_ => new Outbox
                        {
                            Microservice = _.ToString()
                        }).ToList())
                }
            };
            config.Storage.Microservices[microservice.Id.ToString()] = new()
            {
                Shared = new()
                {
                    {
                        "eventStore", new ()
                        {
                            Type = "MongoDB",
                            ConnectionDetails = $"{mongoDBConnectionString}/{GetFirstPartOf(microservice.Id)}-es-shared"
                        }
                    }
                },
                Tenants = new Aksio.Cratis.Configuration.StorageForTenants()
            };

            foreach (var tenant in tenants)
            {
                config.Storage.Microservices[microservice.Id.ToString()].Tenants[tenant.Id.ToString()] = new()
                {
                    {
                        "readModels", new()
                        {
                            Type = "MongoDB",
                            ConnectionDetails = $"{mongoDBConnectionString}/{GetFirstPartOf(microservice.Id)}-{GetFirstPartOf(tenant.Id)}-rm"
                        }
                    },
                    {
                        "eventStore", new()
                        {
                            Type = "MongoDB",
                            ConnectionDetails = $"{mongoDBConnectionString}/{GetFirstPartOf(microservice.Id)}-{GetFirstPartOf(tenant.Id)}-es"
                        }
                    }
                };
            }
        }

        var cratisJson = JsonSerializer.Serialize(config, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        await FileStorage.Upload("cratis.json", cratisJson);
#pragma warning restore // Possible multiple enumerations of IEnumerable
    }

    public async Task CreateAndUploadClientAppSettings(string connectionString, string advertisedClientEndpoint)
    {
        var appSettingsJson = _microservice.AppSettingsContent?.Value ?? "{}";
        var appSettings = JsonNode.Parse(appSettingsJson)!
                            .AsObject()
                            .ConfigureAppSettingsHint()
                            .ConfigureKestrel()
                            .ConfigureLogging()
                            .ConfigureCratisCluster(connectionString, advertisedClientEndpoint);

        await FileStorage.Upload("appsettings.json", appSettings);
    }

    string GetFirstPartOf(Guid guid) => guid.ToString().Split('-')[0];
}
