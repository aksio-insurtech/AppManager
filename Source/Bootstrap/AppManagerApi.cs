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

        Console.WriteLine($"URL for API is : '{_url}'");
    }

    public Task SetStack(Guid id, string json) => Perform($"/api/applications/{id}/stacks", new { json });
    public Task RegisterOrganization(Guid id, string name) => Perform("/api/organizations", new { id = id.ToString(), name });
    public Task SetPulumiSettings(string organization, string accessToken) => Perform("/api/organization/settings/pulumi", new { organization, accessToken });
    public Task SetAzureServicePrincipal(string clientId, string clientSecret) => Perform("/api/organization/settings/service-principal", new { clientId, clientSecret });
    public Task AddAzureSubscription(string id, string name, string tenantId, string tenantName) => Perform("/api/organization/settings/subscriptions", new { id, name, tenantId, tenantName });
    public Task SetMongoDBSettings(string organizationId, string publicKey, string privateKey) => Perform("/api/organization/settings/mongodb", new { organizationId, publicKey, privateKey });
    public Task CreateApplication(Guid applicationId, string name, Guid azureSubscriptionId, string cloudLocation) => Perform("/api/applications", new { applicationId, name, azureSubscriptionId, cloudLocation });
    public Task ConfigureAuthenticationForApplication(Guid applicationId, string clientId, string clientSecret) => Perform($"/api/applications/{applicationId}/authentication", new { clientId, clientSecret });
    public Task CreateMicroservice(Guid applicationId, Guid microserviceId, string name) => Perform($"/api/applications/{applicationId}/microservices", new { microserviceId, name });
    public Task CreateDeployable(Guid applicationId, Guid microserviceId, Guid deployableId, string name) => Perform($"/api/applications/{applicationId}/microservices/{microserviceId}/deployables", new { deployableId, name });
    public Task SetDeployableImage(Guid applicationId, Guid microserviceId, Guid deployableId, string deployableImageName) => Perform($"/api/applications/{applicationId}/microservices/{microserviceId}/deployables/image", new { deployableImageName });

    public void Dispose() => _client.Dispose();

    async Task Perform(string route, object body)
    {
        Console.WriteLine($"Calling : {route}");
        var content = JsonContent.Create(body);
        Console.WriteLine("Post");
        var response = await _client.PostAsync(route, content);
        Console.WriteLine("Done");
        var result = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Result");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(result);
            throw new ApiCallError(route);
        }
    }
}
