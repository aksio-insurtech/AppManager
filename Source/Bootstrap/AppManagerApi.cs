// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Http.Json;
using System.Text.Json;
using Aksio.Execution;
using Concepts.Applications.Environments;
using Concepts.Applications.Environments.Ingresses;
using Concepts.Applications.Tenants;
using MicroserviceId = Concepts.Applications.MicroserviceId;

namespace Bootstrap;

public class AppManagerApi : IDisposable
{
    readonly ManagementConfig _config;
    readonly string _url;
    readonly JsonSerializerOptions _jsonSerializerOptions;
    HttpClient _client;

    public AppManagerApi(ManagementConfig config, string url, JsonSerializerOptions jsonSerializerOptions)
    {
        _config = config;
        _url = url;
        _jsonSerializerOptions = jsonSerializerOptions;
        _client = null!;
    }

    public async Task Authenticate()
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", _config.IdentityProviders.First().ClientId },
            { "client_secret", _config.IdentityProviders.First().ClientSecret },
            { "scope", $"api://{_config.IdentityProviders.First().ClientId}/.default" }
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

    public Task SetPulumiSettings(string organization, string accessToken) => Perform("/api/settings/pulumi", new { organization, accessToken });
    public Task SetAzureServicePrincipal(string clientId, string clientSecret) => Perform("/api/settings/service-principal", new { clientId, clientSecret });
    public Task AddAzureSubscription(string id, string name, string tenantId, string tenantName) => Perform("/api/settings/subscriptions", new { id, name, tenantId, tenantName });
    public Task SetMongoDBSettings(string organizationId, string publicKey, string privateKey) => Perform("/api/settings/mongodb", new { organizationId, publicKey, privateKey });

    public Task CreateApplication(ApplicationId applicationId, string name, Guid azureSubscriptionId, string cloudLocation) => Perform("/api/applications", new { applicationId, name, azureSubscriptionId, cloudLocation });
    public Task CreateEnvironment(ApplicationId applicationId, ApplicationEnvironment environment) => Perform($"/api/applications/{applicationId}/environments", new { environmentId = environment.Id });
    public Task AddTenantToEnvironment(ApplicationId applicationId, ApplicationEnvironment environment, TenantId tenantId, TenantName tenantName) => Perform($"/api/applications/{applicationId}/environments/{environment.Id}/tenants", new { tenantId = environment.Id, name = tenantName });
    public Task CreateIngress(ApplicationId applicationId, ApplicationEnvironment environment, IngressId ingressId, IngressName ingressName) => Perform($"/api/applications/{applicationId}/environments/{environment.Id}/ingresses", new { ingressId, name = ingressName });
    public Task CreateMicroservice(ApplicationId applicationId, ApplicationEnvironment environment, MicroserviceId microserviceId, string name) => Perform($"/api/applications/{applicationId}/environments/{environment.Id}/microservices", new { microserviceId, name });
    public Task CreateDeployable(ApplicationId applicationId, ApplicationEnvironment environment, MicroserviceId microserviceId, Guid deployableId, string name) => Perform($"/api/applications/{applicationId}/environments/{environment.Id}/microservices/{microserviceId}/deployables", new { deployableId, name });
    public Task SetDeployableImage(ApplicationId applicationId, ApplicationEnvironment environment, MicroserviceId microserviceId, Guid deployableId, string deployableImageName) => Perform($"/api/applications/{applicationId}/environments/{environment.Id}/microservices/{microserviceId}/deployables/image", new { deployableImageName });
    public Task SetStackForApplication(ApplicationId id, ApplicationEnvironment environment, string stack) => Perform($"/api/applications/{id}/environments/{environment.Id}/stack", stack);
    public Task SetStackForMicroservice(ApplicationId applicationId, Guid microserviceId, ApplicationEnvironment environment, string stack) => Perform($"/api/applications/{applicationId}/environments/{environment.Id}/microservices/{microserviceId}/stack", stack);

    public void Dispose() => _client.Dispose();

    async Task Perform(string route, object body)
    {
        Console.WriteLine($"Calling : {route}");
        var content = JsonContent.Create(body, options: _jsonSerializerOptions);
        var response = await _client.PostAsync(route, content);
        var result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(result);
            throw new ApiCallError(route);
        }
        Console.WriteLine("API call successful");
    }
}
