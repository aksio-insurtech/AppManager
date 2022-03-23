// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Azure;
using Pulumi;
using Pulumi.AzureNative.OperationalInsights;
using Pulumi.AzureNative.OperationalInsights.Inputs;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web.V20210301;
using Pulumi.AzureNative.Web.V20210301.Inputs;
using ConfigurationArgs = Pulumi.AzureNative.Web.V20210301.Inputs.ConfigurationArgs;
using SecretArgs = Pulumi.AzureNative.Web.V20210301.Inputs.SecretArgs;

namespace Reactions.Applications.Pulumi;

public static class MicroserviceContainerAppPulumiExtensions
{
    public static Task<ContainerAppResult> SetupContainerApp(
        this Microservice microservice,
        Application application,
        ResourceGroup resourceGroup,
        AzureNetworkProfileIdentifier networkProfile,
        string containerRegistryLoginServer,
        string containerRegistryUsername,
        string containerRegistryPassword,
        MicroserviceStorage storage,
        IEnumerable<Deployable> deployables,
        Tags tags)
    {
        var microserviceTags = tags.Clone();
        microserviceTags["microserviceId"] = microservice.Id.ToString();
        microserviceTags["microservice"] = microservice.Name.Value;

        var workspace = new Workspace("loganalytics", new WorkspaceArgs
        {
            Tags = tags,
            ResourceGroupName = resourceGroup.Name,
            Sku = new WorkspaceSkuArgs { Name = WorkspaceSkuNameEnum.PerGB2018 },
            RetentionInDays = 30
        });

        var workspaceSharedKeys = Output.Tuple(resourceGroup.Name, workspace.Name).Apply(items =>
            GetSharedKeys.InvokeAsync(new()
            {
                ResourceGroupName = items.Item1,
                WorkspaceName = items.Item2
            }));

        var kubeEnv = new KubeEnvironment("env", new()
        {
            Tags = tags,
            ResourceGroupName = resourceGroup.Name,
            Location = "westeurope",
            EnvironmentType = "Managed",
            AppLogsConfiguration = new AppLogsConfigurationArgs
            {
                Destination = "log-analytics",
                LogAnalyticsConfiguration = new LogAnalyticsConfigurationArgs
                {
                    CustomerId = workspace.CustomerId,
                    SharedKey = workspaceSharedKeys.Apply(r => r.PrimarySharedKey!)
                }
            }
        });

        var containerApp = new ContainerApp(microservice.Name.Value.ToLowerInvariant(), new()
        {
            Tags = microserviceTags,
            ResourceGroupName = resourceGroup.Name,
            Location = "westeurope",
            KubeEnvironmentId = kubeEnv.Id,
            Configuration = new ConfigurationArgs()
            {
                Ingress = new IngressArgs()
                {
                    External = true,
                    TargetPort = 80
                },
                Registries =
                {
                    new RegistryCredentialsArgs
                    {
                        Server = containerRegistryLoginServer,
                        Username = containerRegistryUsername,
                        PasswordSecretRef = "container-registry"
                    }
                },
                Secrets =
                {
                    new SecretArgs
                    {
                        Name = "container-registry",
                        Value = containerRegistryPassword
                    }
                },
            },
            Template = new TemplateArgs
            {
                Containers = deployables.Select(deployable => new ContainerArgs
                {
                    Name = deployable.Name.Value,
                    Image = deployable.Image,
                }).ToArray()
            }
        });

        return Task.FromResult(new ContainerAppResult(containerApp, string.Empty));
    }
}
