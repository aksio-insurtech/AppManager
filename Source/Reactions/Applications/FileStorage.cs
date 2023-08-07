// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
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

    public FileStorage(string accountName, string accessKey, string shareName, ILogger<FileStorage> logger)
    {
        ConnectionString =
            $"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={accessKey};EndpointSuffix=core.windows.net";
        var shareClient = new ShareClient(ConnectionString, shareName);
        _directoryClient = shareClient.GetDirectoryClient("./");
        AccountName = accountName;
        AccessKey = accessKey;
        ShareName = shareName;

        _logger = logger;
    }

    /// <summary>
    /// Upload the file.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="content">File content.</param>
    /// <param name="skipUploadIfIdenticalFileExists">Skips upload if target is identical, set false to force upload even when target exists and is identical.</param>
    /// <returns>Upload result.</returns>
    public Task<FileStorageResult> Upload(string fileName, JsonNode content, bool skipUploadIfIdenticalFileExists = true)
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        return Upload(fileName, content.ToJsonString(options), skipUploadIfIdenticalFileExists);
    }

    /// <summary>
    /// Upload the file.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="content">File content.</param>
    /// <param name="skipUploadIfIdenticalFileExists">Skips upload if target is identical, set false to force upload even when target exists and is identical.</param>
    /// <returns>Upload result.</returns>
    public Task<FileStorageResult> Upload(string fileName, string content, bool skipUploadIfIdenticalFileExists = true)
    {
        return Upload(fileName, content, skipUploadIfIdenticalFileExists, false);
    }

    /// <summary>
    /// Upload the file.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="content">File content.</param>
    /// <param name="skipUploadIfIdenticalFileExists">Skips upload if target is identical, set false to force upload even when target exists and is identical.</param>
    /// <param name="retrying">Hint if we are in a retry situation, to avoid recursion.</param>
    /// <returns>Upload result.</returns>
    async Task<FileStorageResult> Upload(string fileName, string content, bool skipUploadIfIdenticalFileExists, bool retrying)
    {
        if (content.Length == 0)
        {
            _logger.NoContent(fileName, ShareName);
            return FileStorageResult.NoContent;
        }

        _logger.Uploading(fileName, ShareName);
        var newBytes = Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(newBytes);

        var file = _directoryClient.GetFileClient(fileName);

        try
        {
            // If the existing file exists, check its content if applicable - and then delete if we should.
            if (await file.ExistsAsync())
            {
                if (skipUploadIfIdenticalFileExists)
                {
                    var existingFile = await file.DownloadAsync();

                    byte[] oldBytes;
                    using (var binaryReader = new BinaryReader(existingFile.Value.Content))
                    {
                        oldBytes = binaryReader.ReadBytes((int)stream.Length);
                    }

                    if (ByteArraysEqual(oldBytes, newBytes))
                    {
                        _logger.SkippingUploadOfIdenticalFile(fileName, ShareName);
                        return FileStorageResult.FileExisted;
                    }
                }

                await file.DeleteIfExistsAsync();
            }

            await file.CreateAsync(stream.Length);
            await file.UploadAsync(stream);

            return FileStorageResult.FileUploaded;
        }
        catch (Azure.RequestFailedException rfe)
        {
            // "The specified resource may be in use by an SMB client." error. Wait a little and try again.
            if (!retrying && rfe.Status == 409)
            {
                _logger.RetryingUpload(rfe, fileName, ShareName);
                await Task.Delay(500);
                return await Upload(fileName, content, skipUploadIfIdenticalFileExists, true);
            }

            if (retrying)
            {
                _logger.FailedRetry(rfe, fileName, ShareName);
            }
            else
            {
                _logger.RequestFailedExceptionWhileUploading(rfe, fileName, ShareName);
            }

            throw;
        }
        catch (Exception ex)
        {
            _logger.UnknownExceptionWhileUploading(ex, fileName, ShareName);
            throw;
        }
    }

    /// <summary>
    /// Compares two byte arrays, byte[] is implicitly convertible to ReadOnlySpan of byte.
    /// </summary>
    /// <param name="a1">Array 1.</param>
    /// <param name="a2">Array 2.</param>
    /// <returns>Equals or not.</returns>
    static bool ByteArraysEqual(ReadOnlySpan<byte> a1, ReadOnlySpan<byte> a2)
    {
        return a1.SequenceEqual(a2);
    }
}