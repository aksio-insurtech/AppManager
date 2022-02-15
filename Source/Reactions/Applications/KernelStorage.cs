// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Azure.Storage.Files.Shares;
using Common;
using Pulumi;
using Reactions.Applications.Templates;

namespace Reactions.Applications
{
    public class KernelStorage
    {
        readonly ShareDirectoryClient _directoryClient;

        public KernelStorage(string accountName, string accessKey)
        {
            var connectionString = $"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={accessKey};EndpointSuffix=core.windows.net";
            var share = new ShareClient(connectionString, "kernel");
            _directoryClient = share.GetDirectoryClient("./");
        }

        public void Upload(string fileName, string content)
        {
            if (content.Length == 0)
            {
                Log.Warn($"File '{fileName}' does not have any content - ignoring upload to kernel storage");
                return;
            }

            Log.Info($"Upload file '{fileName}'");

            var file = _directoryClient.GetFileClient(fileName);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            file.DeleteIfExists();
            file.Create(stream.Length);
            file.Upload(stream);
        }

        public void CreateAndUploadStorageJson(string mongoDBConnectionString)
        {
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

            Upload("storage.json", storageJson);
        }

        public void CreateAndUploadAppSettings(Application application, ISettings settings)
        {
            var content = TemplateTypes.AppSettings(
                new AppSettingsValues(application.Name, settings.ElasticUrl, settings.ElasticApiKey));
            Upload("appsettings.json", content);
        }
    }
}
