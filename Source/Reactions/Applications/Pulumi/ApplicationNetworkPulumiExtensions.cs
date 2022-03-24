// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.ManagedIdentity;
using Pulumi.AzureNative.Network;
using Pulumi.AzureNative.Network.Inputs;
using Pulumi.AzureNative.Resources;
using ResourceIdentityType = Pulumi.AzureNative.Network.ResourceIdentityType;

namespace Reactions.Applications.Pulumi;

public static class ApplicationNetworkPulumiExtensions
{
    public static string GetPrivateZoneName(this Application application) => $"{application.Name}.local".ToLowerInvariant();

    public static async Task<NetworkResult> SetupNetwork(this Application application, UserAssignedIdentity identity, Vault vault, ResourceGroup resourceGroup, Tags tags)
    {
        var virtualNetwork = new VirtualNetwork(application.Name, new()
        {
            Location = application.CloudLocation.Value,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            EnableDdosProtection = false,
            EnableVmProtection = false,
            AddressSpace = new AddressSpaceArgs
            {
                AddressPrefixes =
                    {
                        "10.0.0.0/16"
                    }
            },
            Subnets =
                {
                    new global::Pulumi.AzureNative.Network.Inputs.SubnetArgs
                    {
                        Name = "internal",
                        ServiceEndpoints =
                        {
                            new ServiceEndpointPropertiesFormatArgs { Service = "Microsoft.Storage" },
                            new ServiceEndpointPropertiesFormatArgs { Service = "Microsoft.KeyVault" }
                        },
                        AddressPrefix = "10.0.1.0/24",
                        PrivateEndpointNetworkPolicies = "Enabled",
                        PrivateLinkServiceNetworkPolicies = "Enabled",
                        Delegations =
                        {
                            new DelegationArgs
                            {
                                Name = "containerGroupDelegation",
                                ServiceName = "Microsoft.ContainerInstance/containerGroups"
                            }
                        }
                    },
                    new global::Pulumi.AzureNative.Network.Inputs.SubnetArgs
                    {
                        Name = "ApplicationGateway",
                        AddressPrefix = "10.0.2.0/24",
                    }
                }
        });

        var privateZoneName = application.GetPrivateZoneName();
        var privateZone = new PrivateZone("privateZone", new()
        {
            Location = "Global",
            PrivateZoneName = privateZoneName,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
        });

        var virtualNetworkLink = new VirtualNetworkLink("virtualNetworkLink", new()
        {
            Location = "Global",
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            RegistrationEnabled = true,
            PrivateZoneName = privateZone.Name,
            VirtualNetwork = new SubResourceArgs { Id = virtualNetwork.Id }
        });

        var profile = new NetworkProfile(application.Name, new()
        {
            Location = application.CloudLocation.Value,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            ContainerNetworkInterfaceConfigurations =
            {
                new ContainerNetworkInterfaceConfigurationArgs
                {
                    Name = "eth1",
                    IpConfigurations =
                    {
                        new IPConfigurationProfileArgs
                        {
                            Name = "ipconfig1",
                            Subnet = new global::Pulumi.AzureNative.Network.Inputs.SubnetArgs { Id = virtualNetwork.Subnets.Apply(_ => _[0].Id!) }
                        }
                    }
                }
            },
        });

        var publicIPAddress = new PublicIPAddress(application.Name, new()
        {
            Location = application.CloudLocation.Value,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            PublicIPAddressVersion = "IPv4",
            PublicIPAllocationMethod = "Static",
            PublicIpAddressName = "public",
            Sku = new PublicIPAddressSkuArgs
            {
                Name = PublicIPAddressSkuName.Standard,
                Tier = PublicIPAddressSkuTier.Regional
            }
        });

        var applicationGatway = await application.SetupApplicationGateway(virtualNetwork, publicIPAddress, identity, vault, resourceGroup, tags);

        return new(virtualNetwork, profile, privateZone, publicIPAddress, applicationGatway);
    }

    static async Task<ApplicationGateway> SetupApplicationGateway(this Application application, VirtualNetwork virtualNetwork, PublicIPAddress publicIPAddress, UserAssignedIdentity identity, Vault vault, ResourceGroup resourceGroup, Tags tags)
    {
        var keyResult = GetKey.Invoke(new()
        {
            ResourceGroupName = resourceGroup.Name,
            VaultName = vault.Name,
            KeyName = "opensjon"
        });

        var keyId = keyResult.Apply(_ => _.Id);

        var applicationGatewayName = $"{application.Name}";
        const string frontendIPConfigurationName = "frontend-ip";
        const string frontendPortName = "https-port";
        const string frontendUnsecurePortName = "http-port";
        const string ingressAddressPoolsName = "ingress-pool";
        const string ingressHttpSettingsName = "ingress-http";
        const string httpListenerName = "https-listener";
        const string httpUnsecureListenerName = "http-listener";
        const string sslCertificateName = "ssl-cert";
        const string sslProfileName = "ssl-profile";

        var resourceGroupId = resourceGroup.Id.Apply(id => id);

        var frontendIPConfigurationId = Output.Format($"{resourceGroup.Id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/frontendIPConfigurations/{frontendIPConfigurationName}");
        var frontendPortId = Output.Format($"{resourceGroup.Id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/frontendPorts/{frontendPortName}");
        var frontendUnsecurePortId = Output.Format($"{resourceGroup.Id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/frontendPorts/{frontendUnsecurePortName}");
        var backendAddressPoolsId = Output.Format($"{resourceGroup.Id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/backendAddressPools/{ingressAddressPoolsName}");
        var backendHttpSettingsId = Output.Format($"{resourceGroup.Id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/backendHttpSettingsCollection/{ingressHttpSettingsName}");
        var httpListenerId = Output.Format($"{resourceGroup.Id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/httpListeners/{httpListenerName}");
        var httpUnsecureListenerId = Output.Format($"{resourceGroup.Id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/httpListeners/{httpUnsecureListenerName}");
        var sslCertificateId = Output.Format($"{resourceGroup.Id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/sslCertificates/{sslCertificateName}");
        var sslProfileId = Output.Format($"{resourceGroup.Id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/sslProfiles/{sslProfileName}");

        var identityIdOutput = Output.Format($"{resourceGroup.Id}/providers/Microsoft.ManagedIdentity/userAssignedIdentities/{identity.Name}");
        var identityId = await identityIdOutput.GetValue();

        return new ApplicationGateway(application.Name, new()
        {
            ApplicationGatewayName = applicationGatewayName,
            Location = application.CloudLocation.Value,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            EnableHttp2 = true,
            Identity = new ManagedServiceIdentityArgs
            {
                Type = ResourceIdentityType.UserAssigned,
                UserAssignedIdentities =
                {
                    { identityId, new Dictionary<string, object>() }
                }
            },
            Sku = new ApplicationGatewaySkuArgs
            {
                Name = ApplicationGatewaySkuName.Standard_v2,
                Tier = ApplicationGatewayTier.Standard_v2,
                Capacity = 2
            },
            BackendAddressPools =
            {
                new ApplicationGatewayBackendAddressPoolArgs
                {
                    Name = ingressAddressPoolsName,
                    BackendAddresses =
                    {
                        new ApplicationGatewayBackendAddressArgs
                        {
                            IpAddress = "ingress.myapplication.local"
                        }
                    }
                }
            },
            BackendHttpSettingsCollection =
            {
                new ApplicationGatewayBackendHttpSettingsArgs
                {
                    Name = ingressHttpSettingsName,
                    CookieBasedAffinity = "Disabled",
                    Port = 80,
                    Protocol = ApplicationGatewayProtocol.Http,
                    RequestTimeout = 30
                }
            },
            GatewayIPConfigurations =
            {
                new ApplicationGatewayIPConfigurationArgs
                {
                    Name = "ip-configuration",
                    Subnet = new SubResourceArgs { Id = virtualNetwork.Subnets.Apply(_ => _[1].Id!) }
                }
            },
            FrontendPorts =
            {
                new ApplicationGatewayFrontendPortArgs
                {
                    Name = frontendPortName,
                    Port = 443
                },
                new ApplicationGatewayFrontendPortArgs
                {
                    Name = frontendUnsecurePortName,
                    Port = 80
                }
            },
            FrontendIPConfigurations =
            {
                new ApplicationGatewayFrontendIPConfigurationArgs
                {
                    Name = frontendIPConfigurationName,
                    PublicIPAddress = new SubResourceArgs { Id = publicIPAddress.Id }
                }
            },
            HttpListeners =
            {
                new ApplicationGatewayHttpListenerArgs
                {
                    Name = httpListenerName,
                    HostNames =
                    {
                        "dev.opensjon.aksio.app"
                    },
                    FrontendIPConfiguration = new SubResourceArgs
                    {
                        Id = frontendIPConfigurationId
                    },
                    FrontendPort = new SubResourceArgs
                    {
                        Id = frontendPortId
                    },
                    Protocol = ApplicationGatewayProtocol.Https,
                    SslCertificate = new SubResourceArgs
                    {
                        Id = sslCertificateId
                    },
                    SslProfile = new SubResourceArgs
                    {
                        Id = sslProfileId
                    }
                },
                new ApplicationGatewayHttpListenerArgs
                {
                    Name = httpUnsecureListenerName,
                    HostNames =
                    {
                        "dev.opensjon.aksio.app"
                    },
                    FrontendIPConfiguration = new SubResourceArgs
                    {
                        Id = frontendIPConfigurationId
                    },
                    FrontendPort = new SubResourceArgs
                    {
                        Id = frontendUnsecurePortId
                    },
                    Protocol = ApplicationGatewayProtocol.Http
                }
            },
            RequestRoutingRules =
            {
                new ApplicationGatewayRequestRoutingRuleArgs
                {
                    Name = "https-rule",
                    RuleType = ApplicationGatewayRequestRoutingRuleType.Basic,
                    Priority = 10,
                    BackendAddressPool = new SubResourceArgs { Id = backendAddressPoolsId },
                    BackendHttpSettings = new SubResourceArgs { Id = backendHttpSettingsId },
                    HttpListener = new SubResourceArgs { Id = httpListenerId },
                },
                new ApplicationGatewayRequestRoutingRuleArgs
                {
                    Name = "http-rule",
                    RuleType = ApplicationGatewayRequestRoutingRuleType.Basic,
                    Priority = 11,
                    BackendAddressPool = new SubResourceArgs { Id = backendAddressPoolsId },
                    BackendHttpSettings = new SubResourceArgs { Id = backendHttpSettingsId },
                    HttpListener = new SubResourceArgs { Id = httpUnsecureListenerId },
                }
            },
            SslCertificates =
            {
                new ApplicationGatewaySslCertificateArgs
                {
                    Name = sslCertificateName,
                    KeyVaultSecretId = "https://myapplication-vault.vault.azure.net/secrets/opensjon"
                }
            },
            SslProfiles =
            {
                new ApplicationGatewaySslProfileArgs
                {
                    Name = sslProfileName,
                    ClientAuthConfiguration = new ApplicationGatewayClientAuthConfigurationArgs
                    {
                        VerifyClientCertIssuerDN = true
                    },
                    SslPolicy = new ApplicationGatewaySslPolicyArgs
                    {
                        PolicyType = ApplicationGatewaySslPolicyType.Predefined,
                        PolicyName = ApplicationGatewaySslPolicyName.AppGwSslPolicy20170401
                    }
                }
            }
        });
    }
}
