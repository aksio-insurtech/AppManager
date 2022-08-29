// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Http.Json;
using System.Text.Json;

namespace Bootstrap;

public class AppManagerApi : IDisposable
{
    readonly ManagementConfig _config;
    readonly string _url;
    HttpClient _client;

    public AppManagerApi(ManagementConfig config, string url)
    {
        _config = config;
        _url = url;
        _client = null!;
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

        var client = new HttpClient();
        var response = await client.PostAsync($"https://login.microsoftonline.com/{_config.Azure.TenantId}/oauth2/v2.0/token", content);
        var result = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(result);
        var accessToken = document.RootElement.GetProperty("access_token").GetString()!;

        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        _client.BaseAddress = new Uri(_url);
    }

    public Task RegisterOrganization(Guid id, string name) => Perform("/api/organizations", new { id = id.ToString(), name });

    public void Dispose() => _client.Dispose();

    async Task Perform(string route, object body)
    {
        var content = JsonContent.Create(body);
        var response = await _client.PostAsync(route, content);
        var result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(result);
            throw new ApiCallError(route);
        }
    }
}
