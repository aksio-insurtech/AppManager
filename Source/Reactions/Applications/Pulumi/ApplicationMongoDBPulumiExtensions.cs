// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Common;
using Concepts;
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

        var cluster = new Cluster(environment.GetStackNameFor(), new ClusterArgs
        {
            ProjectId = project.Id,
            ProviderName = "TENANT",
            BackingProviderName = "AZURE",
            ProviderInstanceSizeName = "M0",
            ProviderRegionName = "EUROPE_NORTH",
            Labels = tags.Select((kvp) => new ClusterLabelArgs { Key = kvp.Key, Value = kvp.Value }).ToArray()
        });

        var databasePassword = Guid.NewGuid().ToString();
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

            // Todo: Only accept IP addresses from the actual running Microservices or Vnet or something
            // IpAddress = container.IpAddress.Apply(_ => _!.Ip!)
            IpAddress = "0.0.0.0"
        });

        var connectionStrings = await cluster.ConnectionStrings.GetValue();
        var connectionString = connectionStrings[0].StandardSrv ?? string.Empty;

        const string scheme = "mongodb+srv://";
        var mongoDBConnectionString = connectionString.Insert(scheme.Length, $"kernel:{databasePassword}@");

        return new(cluster, connectionString, databasePassword);
    }
}
