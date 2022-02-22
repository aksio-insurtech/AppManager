// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Azure.Storage.Files.Shares;
using Common;
using Microsoft.Extensions.Logging;
using Pulumi;
using Reactions.Applications.Templates;

namespace Reactions.Applications
{
    public class MicroserviceStorage
    {
        readonly ShareDirectoryClient _directoryClient;
        readonly Application _application;
        readonly Microservice _microservice;
        readonly ILogger<MicroserviceStorage> _logger;

        public string AccountName { get; }
        public string AccessKey { get; }
        public string ShareName { get; }

        public MicroserviceStorage(
            Application application,
            Microservice microservice,
            string accountName,
            string accessKey,
            string shareName,
            ILogger<MicroserviceStorage> logger)
        {
            var connectionString = $"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={accessKey};EndpointSuffix=core.windows.net";
            var shareClient = new ShareClient(connectionString, shareName);
            _directoryClient = shareClient.GetDirectoryClient("./");
            _application = application;
            _microservice = microservice;
            AccountName = accountName;
            AccessKey = accessKey;
            ShareName = shareName;
            _logger = logger;
        }

        public void Upload(string fileName, string content)
        {
            if (content.Length == 0)
            {
                Log.Warn($"File '{fileName}' does not have any content - ignoring upload to kernel storage");
                return;
            }

            _logger.Uploading(_application.Name, _microservice.Name, fileName, ShareName);

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

        public void CreateAndUploadAppSettings(ISettings settings)
        {
            var content = TemplateTypes.AppSettings(
                new AppSettingsValues(_application.Name, _microservice.Name, settings.ElasticUrl, settings.ElasticApiKey));
            Upload("appsettings.json", content);
        }
    }
}
