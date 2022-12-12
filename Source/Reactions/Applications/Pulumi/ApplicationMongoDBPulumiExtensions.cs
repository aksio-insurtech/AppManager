// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Common;
using Concepts.Applications;
using Pulumi;
using Pulumi.AzureNative.Network;
using Pulumi.AzureNative.Network.Inputs;
using Pulumi.AzureNative.Resources;
using Pulumi.Mongodbatlas;
using Pulumi.Mongodbatlas.Inputs;
using SubnetArgs = Pulumi.AzureNative.Network.Inputs.SubnetArgs;

namespace Reactions.Applications.Pulumi;

public static class ApplicationMongoDBPulumiExtensions
{
    public static async Task<MongoDBResult> SetupMongoDB(
        this Application application,
        ISettings settings,
        ResourceGroup resourceGroup,
        VirtualNetwork vnet,
        ApplicationEnvironmentWithArtifacts environment,
        Tags tags)
    {
        var mongoDBOrganizationId = settings.MongoDBOrganizationId;

        var project = new Project(application.Name, new ProjectArgs
        {
            OrgId = mongoDBOrganizationId.Value
        });

        var region = GetRegionName(environment.CloudLocation);

        var privateLinkEndpoint = new PrivateLinkEndpoint(application.Name, new()
        {
            ProjectId = project.Id,
            ProviderName = "AZURE",
            Region = region
        });

        var endpointId = await privateLinkEndpoint.Id.GetValue();

        var subnet = vnet.Subnets.Apply(_ => _.First(s => s.Name == "mongodb"));

        var privateEndpoint = new PrivateEndpoint(application.Name, new()
        {
            Location = environment.CloudLocation.Value,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            Subnet = new SubnetArgs
            {
                Id = subnet.Apply(_ => _.Id!)
            },
            ManualPrivateLinkServiceConnections =
            {
                new PrivateLinkServiceConnectionArgs
                {
                    GroupIds = privateLinkEndpoint.EndpointGroupNames,
                    Name = "MongoDB",
                    PrivateLinkServiceId = privateLinkEndpoint.PrivateLinkServiceResourceId,
                    RequestMessage = "Please approve my connection",
                }
            }
        });

        var networkInterfaces = await privateEndpoint.NetworkInterfaces.GetValue();
        var resourceGroupName = await resourceGroup.Name.GetValue();
        var networkInterface = await GetNetworkInterface.InvokeAsync(new()
        {
            NetworkInterfaceName = networkInterfaces[0].Id!.Substring(networkInterfaces[0].Id!.LastIndexOf("/") + 1),
            ResourceGroupName = resourceGroupName
        });

        _ = new PrivateLinkEndpointService(application.Name, new()
        {
            ProjectId = project.Id,
            PrivateLinkId = privateLinkEndpoint.PrivateLinkId,
            EndpointServiceId = privateEndpoint.Id,
            ProviderName = "AZURE",
            PrivateEndpointIpAddress = networkInterface.IpConfigurations[0].PrivateIPAddress!
        });

        var clusterName = $"{application.Name}-{environment.DisplayName}".ToLowerInvariant();
        var cluster = new Cluster(clusterName, new ClusterArgs
        {
            ProjectId = project.Id,
            ProviderName = "AZURE",
            ProviderInstanceSizeName = "M10",
            ProviderRegionName = region,
            Labels = tags.Select((kvp) => new ClusterLabelArgs { Key = kvp.Key, Value = kvp.Value }).ToArray()
        });

        var databasePassword = Guid.NewGuid().ToString();
        if (environment.ApplicationResources?.MongoDB?.Users is not null &&
            (environment.ApplicationResources?.MongoDB?.Users.Any(_ => _.UserName == "kernel") ?? false))
        {
            databasePassword = environment.ApplicationResources?.MongoDB?.Users.First(_ => _.UserName == "kernel").Password ?? databasePassword;
        }

        _ = new DatabaseUser("kernel", new()
        {
            Username = "kernel",
            ProjectId = cluster.ProjectId,
            Password = databasePassword,
            DatabaseName = "admin",
            Roles = new DatabaseUserRoleArgs[]
            {
                new ()
                {
                    DatabaseName = "admin",
                    RoleName = "readWriteAnyDatabase"
                }
            }
        });

        var connectionStrings = await cluster.ConnectionStrings.GetValue();
        var connectionString = connectionStrings[0].StandardSrv ?? string.Empty;
        return new(cluster, connectionString, databasePassword);
    }

    static string GetRegionName(CloudLocationKey cloudLocation) => cloudLocation.Value switch
    {
        CloudLocationKey.NorwayEast => "NORWAY_EAST",
        CloudLocationKey.EuropeWest => "EUROPE_WEST",
        CloudLocationKey.EuropeNorth => "EUROPE_NORTH",
        _ => string.Empty
    };
}
