// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Common;
using Concepts;
using Concepts.Applications;
using Pulumi.Mongodbatlas;
using Pulumi.Mongodbatlas.Inputs;

namespace Reactions.Applications.Pulumi;

public static class ApplicationMongoDBPulumiExtensions
{
    public static async Task<MongoDBResult> SetupMongoDB(this Application application, ISettings settings, CloudRuntimeEnvironment environment, Tags tags)
    {
        var mongoDBOrganizationId = settings.MongoDBOrganizationId;

        var project = new Project(application.Name, new ProjectArgs
        {
            OrgId = mongoDBOrganizationId.Value
        });

        var clusterName = $"{application.Name}-{environment.ToDisplayName()}".ToLowerInvariant();
        var cluster = new Cluster(clusterName, new ClusterArgs
        {
            ProjectId = project.Id,
            ProviderName = "AZURE",
            ProviderInstanceSizeName = "M10",
            ProviderRegionName = GetRegionName(application.CloudLocation),
            Labels = tags.Select((kvp) => new ClusterLabelArgs { Key = kvp.Key, Value = kvp.Value }).ToArray()
        });

        var databasePassword = Guid.NewGuid().ToString();
        if (application.Resources?.MongoDB?.Users is not null &&
            (application.Resources?.MongoDB?.Users.Any(_ => _.UserName == "kernel") ?? false))
        {
            databasePassword = application.Resources?.MongoDB?.Users.First(_ => _.UserName == "kernel").Password;
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

        _ = new ProjectIpAccessList("kernel", new()
        {
            ProjectId = project.Id,
            IpAddress = "0.0.0.0"
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
