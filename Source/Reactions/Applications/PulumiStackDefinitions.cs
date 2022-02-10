// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.Automation;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using Pulumi.Mongodbatlas;

namespace Reactions.Applications
{
    public class PulumiStackDefinitions : IPulumiStackDefinitions
    {
        public PulumiFn CreateApplication(Application application, RuntimeEnvironment environment) =>

            // Notes:
            // - Organization settings: Atlas OrgId
            // - Create MongoDB cluster for application
            // - Create MongoDB database user
            // - Automatically create dev & prod stacks
            // - Setup Kernel container instance
            // - Add tag for application for each stack
            // - Add tag for environment for each stack
            // - Store needed output values - show on resources tab
            // - Output
            PulumiFn.Create(() =>
            {
                var resourceGroup = ResourceGroup.Get("Einar-D-Norway-RG", $"/subscriptions/{application.AzureSubscriptionId}/resourceGroups/Einar-D-Norway-RG");
                var storageAccount = new StorageAccount(application.Name.Value.ToLowerInvariant(), new StorageAccountArgs
                {
                    ResourceGroupName = resourceGroup.Name,
                    Tags = new Dictionary<string, string>
                    {
                        { "application", application.Id.ToString() }
                    },
                    Sku = new SkuArgs
                    {
                        Name = SkuName.Standard_LRS
                    },
                    Kind = Kind.StorageV2
                });

                var project = new Project(application.Name, new ProjectArgs
                {
                    OrgId = "61f150d98bc1f86a0748984d"
                });

                var cluster = new Cluster("dev", new ClusterArgs
                {
                    ProjectId = project.Id,
                    ProviderName = "TENANT",
                    BackingProviderName = "AZURE",
                    ProviderInstanceSizeName = "M0",
                    ProviderRegionName = "EUROPE_NORTH"
                });
            });

        public PulumiFn CreateDeployable(RuntimeEnvironment environment) => throw new NotImplementedException();
        public PulumiFn CreateMicroservice(RuntimeEnvironment environment) => throw new NotImplementedException();
    }
}
