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

        var project = new Project($"{application.Name}-{environment.ShortName}", new ProjectArgs
        {
            OrgId = mongoDBOrganizationId.Value
        });

        var region = environment.CloudLocation.ToAtlas();

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
            CloudBackup = environment.BackupEnabled,
            PitEnabled = environment.BackupEnabled,
            Labels = tags.Select((kvp) => new ClusterLabelArgs { Key = kvp.Key, Value = kvp.Value }).ToArray()
        });

        var databasePassword = Guid.NewGuid().ToString();
        if (environment.MongoDB?.Users is not null &&
            (environment.MongoDB?.Users.Any(_ => _.UserName == "kernel") ?? false))
        {
            databasePassword = environment.MongoDB?.Users.First(_ => _.UserName == "kernel").Password ?? databasePassword;
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

        if (environment.BackupEnabled)
        {
            var replicationSpecId = cluster.ReplicationSpecs.Apply(_ => _[0].Id!);

            _ = new CloudBackupSchedule("backup", new()
            {
                ProjectId = cluster.ProjectId,
                ClusterName = cluster.Name,
                ReferenceHourOfDay = 3,
                ReferenceMinuteOfHour = 45,
                RestoreWindowDays = 7,
                CopySettings = new CloudBackupScheduleCopySettingArgs[]
                {
                    new ()
                    {
                        CloudProvider = "AZURE",
                        Frequencies = "DAILY",
                        RegionName = environment.BackupCopyRegion.ToAtlas(),
                        ShouldCopyOplogs = true,
                        ReplicationSpecId = replicationSpecId
                    }
                },
                PolicyItemHourly = new CloudBackupSchedulePolicyItemHourlyArgs
                {
                    FrequencyInterval = 1,
                    RetentionUnit = "days",
                    RetentionValue = 7 // Keep 7 days of hourly backups, value can't be less than the `RestoreWindowDays` value
                },
                PolicyItemDaily = new CloudBackupSchedulePolicyItemDailyArgs
                {
                    FrequencyInterval = 1,
                    RetentionUnit = "days",
                    RetentionValue = 7
                },
                PolicyItemWeeklies = new CloudBackupSchedulePolicyItemWeeklyArgs[]
                {
                    new ()
                    {
                        FrequencyInterval = 7,  // Sunday
                        RetentionUnit = "weeks",
                        RetentionValue = 4
                    }
                },
                PolicyItemMonthlies = new CloudBackupSchedulePolicyItemMonthlyArgs[]
                {
                    new ()
                    {
                        FrequencyInterval = 1,  // First day of month
                        RetentionUnit = "months",
                        RetentionValue = 12
                    }
                }
            });
        }

        var connectionStrings = await cluster.ConnectionStrings.GetValue();
        var connectionString = connectionStrings[0].PrivateEndpoints[0].SrvConnectionString ?? string.Empty;
        return new(cluster, connectionString, databasePassword);
    }
}
