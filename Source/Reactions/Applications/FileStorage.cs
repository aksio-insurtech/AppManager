// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using Azure.Storage.Files.Shares;
using Microsoft.Extensions.Logging;

namespace Reactions.Applications;

public class FileStorage
{
    readonly ShareDirectoryClient _directoryClient;
    readonly ILogger<FileStorage> _logger;

    public string ConnectionString { get; }
    public string AccountName { get; }
    public string AccessKey { get; }
    public string ShareName { get; }

    public FileStorage(
        string accountName,
        string accessKey,
        string shareName,
        ILogger<FileStorage> logger)
    {
        ConnectionString = $"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={accessKey};EndpointSuffix=core.windows.net";
        var shareClient = new ShareClient(ConnectionString, shareName);
        _directoryClient = shareClient.GetDirectoryClient("./");
        AccountName = accountName;
        AccessKey = accessKey;
        ShareName = shareName;

        _logger = logger;
    }

    public void Upload(string fileName, string content)
    {
        if (content.Length == 0)
        {
            _logger.NoContent(fileName, ShareName);
            return;
        }

        _logger.Uploading(fileName, ShareName);

        var file = _directoryClient.GetFileClient(fileName);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        file.DeleteIfExists();
        file.Create(stream.Length);
        file.Upload(stream);
    }
}
