// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Http.Json;
using System.Text.Json;

namespace Bootstrap;

public class AppManagerApi : IDisposable
{
    readonly ManagementConfig _config;
    readonly string _url;
    readonly HttpClient _client;

    public AppManagerApi(ManagementConfig config, string url)
    {
        _config = config;
        _url = url;
        _client = new HttpClient();
    }

    public async Task Authenticate()
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", _config.Authentication.ClientId },
            { "client_secret", _config.Authentication.ClientSecret },
            { "scope", $"api://{_config.Authentication.ClientId}/.default" }
        });

        var response = await _client.PostAsync($"https://login.microsoftonline.com/{_config.Azure.TenantId}/oauth2/v2.0/token", content);
        var result = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(result);
        var accessToken = document.RootElement.GetProperty("access_token").GetString()!;
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
    }

    public Task RegisterOrganization(Guid id, string name) => Perform("/api/organization", new { id = id.ToString(), name });

    public void Dispose() => _client.Dispose();

    async Task Perform(string route, object body)
    {
        var content = JsonContent.Create(body);
        var url = $"{_url}/{route}";
        var response = await _client.PostAsync(url, content);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            throw new ApiCallError(url);
        }
    }
}
