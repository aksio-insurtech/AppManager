// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Nodes;
using Aksio.Cratis.Configuration;

namespace Reactions.Applications;

public static class AppSettingsExtensions
{
    public static JsonObject ConfigureKestrel(this JsonObject json)
    {
        json["Kestrel"] = new JsonObject()
        {
            ["Endpoints"] = new JsonObject()
            {
                ["Http"] = new JsonObject()
                {
                    ["Url"] = "http://+:80"
                }
            }
        };

        return json;
    }

    public static JsonObject ConfigureLogging(this JsonObject json)
    {
        json["Serilog"] = new JsonObject()
        {
            ["MinimumLevel"] = new JsonObject()
            {
                ["Default"] = "Information",
                ["Override"] = new JsonObject()
                {
                    ["Aksio"] = "Information",
                    ["Microsoft"] = "Warning",
                    ["Microsoft.AspNetCore.HttpLogging"] = "Warning",
                    ["System"] = "Warning"
                }
            },
            ["WriteTo"] = new JsonArray()
            {
                new JsonObject()
                {
                    ["Name"] = "Console",
                    ["Args"] = new JsonObject()
                    {
                        ["formatter"] = "Aksio.Applications.Serilog.RenderedCompactJsonFormatter, Aksio.Applications.Serilog@"
                    }
                }
            }
        };
        json["AllowedHosts"] = "*";
        return json;
    }

    public static JsonObject ConfigureCratisCluster(this JsonObject json, string connectionString, string advertisedClientEndpoint)
    {
        json["cratis"] = new JsonObject()
        {
            [nameof(ClientOptions.Kernel)] = new JsonObject()
            {
                [nameof(KernelConnectivity.AzureStorageCluster)] = new JsonObject()
                {
                    [nameof(AzureStorageClusterOptions.ConnectionString)] = connectionString,
                    [nameof(AzureStorageClusterOptions.Secure)] = false,
                    [nameof(AzureStorageClusterOptions.Port)] = 80,
                    [nameof(AzureStorageClusterOptions.TableName)] = AzureStorageClusterOptions.DEFAULT_TABLE_NAME
                },
                [nameof(KernelConnectivity.AdvertisedClientEndpoint)] = advertisedClientEndpoint
            }
        };

        return json;
    }
}
