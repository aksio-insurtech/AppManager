// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Reactions.Applications;

public static class AppSettingsUtils
{

    public static string OverrideLogging(string existing)
    {
        var json = JsonNode.Parse(existing)!.AsObject();
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
                        ["formatter"] = "Serilog.Formatting.Json.JsonFormatter, Serilog"
                    }
                }
            }
        };
        json["AllowedHosts"] = "*";

        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };
        return json.ToJsonString(options);
    }
}
