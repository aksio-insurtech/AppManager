// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Common;
using Concepts;
using Concepts.Applications;
using Pulumi;
using Pulumi.AzureNative.Network;
using Pulumi.AzureNative.Resources;
using Pulumi.Mongodbatlas;
using Pulumi.Mongodbatlas.Inputs;

namespace Reactions.Applications.Pulumi;

public static class ApplicationMongoDBPulumiExtensions
{
    public static async Task<MongoDBResult> SetupMongoDB(this Application application, ISettings settings, ResourceGroup resourceGroup, VirtualNetwork vnet, CloudRuntimeEnvironment environment, Tags tags)
    {
        var mongoDBOrganizationId = settings.MongoDBOrganizationId;

        var project = new Project(application.Name, new ProjectArgs
        {
            OrgId = mongoDBOrganizationId.Value
        });

        var region = GetRegionName(application.CloudLocation);

        /*
        var current = await global::Pulumi.AzureAD.GetClientConfig.InvokeAsync();
        var peeringApplication = new global::Pulumi.AzureAD.Application(
            "MongoDBAtlasVNETPeering",
            new()
            {
                DisplayName = "example",
                Owners = new[]
                {
                    current.ObjectId,
                }
            },
            new()
            {
                Id = "e90a1407-553c-432d-9cb1-3638900a9d22"
            });

        _ = new global::Pulumi.AzureAD.ServicePrincipal("MongoDBAtlasVNETPeering", new()
        {
            ApplicationId = "e90a1407-553c-432d-9cb1-3638900a9d22",
            AppRoleAssignmentRequired = false,
            Owners = new[]
            {
                current.ObjectId
            }
        });

        var resourceGroupName = await resourceGroup.Name.GetValue();
        var vnetName = await vnet.Name.GetValue();
        var roleDefinitionName = $"AtlasPeering/{application.AzureSubscriptionId}/{resourceGroupName}/{vnetName}";
        var scopeName = $"/subscriptions/{application.AzureSubscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Network/virtualNetworks/{vnetName}";
        var roleDefinition = new RoleDefinition(roleDefinitionName, new()
        {
            RoleName = roleDefinitionName,
            Description = $"Grants MongoDB access to manage peering connections on network /subscriptions/{application.AzureSubscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Network/virtualNetworks/{vnetName}",
            Permissions =
            {
                new PermissionArgs()
                {
                    Actions =
                    {
                        "Microsoft.Network/virtualNetworks/virtualNetworkPeerings/read",
                        "Microsoft.Network/virtualNetworks/virtualNetworkPeerings/write",
                        "Microsoft.Network/virtualNetworks/virtualNetworkPeerings/delete",
                        "Microsoft.Network/virtualNetworks/peer/action"
                    },
                }
            },
            AssignableScopes =
            {
                scopeName
            }
        });

        _ = new RoleAssignment("MongoDBAtlasPeering", new()
        {
            RoleAssignmentName = "MongoDBAtlasPeering",
            RoleDefinitionId = roleDefinition.Id,
            Scope = scopeName
        });

        var getNetworkContainersResult = GetNetworkContainers.Invoke(new()
        {
            ProjectId = project.Id,
            ProviderName = "Azure"
        });
        var networkContainers = await getNetworkContainersResult.GetValue();
        _ = new NetworkPeering(application.Name, new()
        {
            ProjectId = project.Id,
            ContainerId = networkContainers.Results[0].Id,
            AzureDirectoryId = settings.AzureSubscriptions.First().TenantId.Value,
            AzureSubscriptionId = settings.AzureSubscriptions.First().SubscriptionId.Value.ToString(),
            ResourceGroupName = resourceGroup.Name,
            ProviderName = "AZURE",
            RouteTableCidrBlock = "10.0.1.0/24",
            VnetName = vnet.Name
        });
        */

        var clusterName = $"{application.Name}-{environment.ToDisplayName()}".ToLowerInvariant();
        var cluster = new Cluster(clusterName, new ClusterArgs
        {
            ProjectId = project.Id,
            ProviderName = "AZURE",
            ProviderInstanceSizeName = "M10",
            ProviderRegionName = region,
            Labels = tags.Select((kvp) => new ClusterLabelArgs { Key = kvp.Key, Value = kvp.Value }).ToArray()
        });

        // TODO: We need this for the cluster:
        // , new CustomResourceOptions
        // {
        //     DependsOn =
        //     {
        //         "mongodbatlas_network_peering.test",
        //     },
        // });
        // https://www.pulumi.com/registry/packages/mongodbatlas/api-docs/networkpeering/#example-with-azure
        var databasePassword = Guid.NewGuid().ToString();
        if (application.Resources?.MongoDB?.Users is not null &&
            (application.Resources?.MongoDB?.Users.Any(_ => _.UserName == "kernel") ?? false))
        {
            databasePassword = application.Resources?.MongoDB?.Users.First(_ => _.UserName == "kernel").Password ?? databasePassword;
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

        // _ = new ProjectIpAccessList("kernel", new()
        // {
        //     ProjectId = project.Id,
        //     IpAddress = "0.0.0.0"
        // });
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
