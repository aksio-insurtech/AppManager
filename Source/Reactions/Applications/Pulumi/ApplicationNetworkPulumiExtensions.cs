// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;
using Pulumi.AzureNative.ManagedIdentity;
using Pulumi.AzureNative.Network;
using Pulumi.AzureNative.Network.Inputs;
using Pulumi.AzureNative.Resources;
using ResourceIdentityType = Pulumi.AzureNative.Network.ResourceIdentityType;
using VaultSkuArgs = Pulumi.AzureNative.KeyVault.Inputs.SkuArgs;
using VaultSkuName = Pulumi.AzureNative.KeyVault.SkuName;

namespace Reactions.Applications.Pulumi;

public static class ApplicationNetworkPulumiExtensions
{
    public static string GetPrivateZoneName(this Application application) => $"{application.Name}.local".ToLowerInvariant();

    public static NetworkResult SetupNetwork(this Application application, ResourceGroup resourceGroup, Tags tags)
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
                            new ServiceEndpointPropertiesFormatArgs
                            {
                                Service = "Microsoft.Storage"
                            },
                            new ServiceEndpointPropertiesFormatArgs
                            {
                                Service = "Microsoft.KeyVault"
                            }
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
                        Name = "AzureFirewallSubnet",
                        AddressPrefix = "10.0.2.0/24",
                    },
                    new global::Pulumi.AzureNative.Network.Inputs.SubnetArgs
                    {
                        Name = "LeGateway",
                        AddressPrefix = "10.0.3.0/24",
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
            VirtualNetwork = new SubResourceArgs
            {
                Id = virtualNetwork.Id
            }
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
                            Subnet = new global::Pulumi.AzureNative.Network.Inputs.SubnetArgs
                            {
                                Id = virtualNetwork.Subnets.Apply(_ => _[0].Id!)
                            }
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

        var secondPublicIPAddress = new PublicIPAddress($"{application.Name}-second", new()
        {
            Location = application.CloudLocation.Value,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            PublicIPAddressVersion = "IPv4",
            PublicIPAllocationMethod = "Static",
            PublicIpAddressName = "second-public",
            Sku = new PublicIPAddressSkuArgs
            {
                Name = PublicIPAddressSkuName.Standard,
                Tier = PublicIPAddressSkuTier.Regional
            }
        });

        var identityName = $"{application.Name}-user";
        var identity = new UserAssignedIdentity(identityName, new()
        {
            Location = application.CloudLocation.Value,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
        });

        var identityId = resourceGroup.Id.Apply(id =>
            $"{id}/providers/Microsoft.ManagedIdentity/userAssignedIdentities/{identityName}");

        var vault = new Vault(application.Name.Value, new()
        {
            Location = application.CloudLocation.Value,
            ResourceGroupName = resourceGroup.Name,
            Tags = tags,
            VaultName = $"{application.Name}-vault",
            Properties = new VaultPropertiesArgs
            {
                AccessPolicies =
                {
                    new AccessPolicyEntryArgs
                    {
                        ObjectId = "ffeafe73-d0ab-495f-ac77-c64277f45a4f",
                        Permissions = new PermissionsArgs
                        {
                            Certificates =
                            {
                                CertificatePermissions.Get,
                                CertificatePermissions.List,
                                CertificatePermissions.Delete,
                                CertificatePermissions.Create,
                                CertificatePermissions.Import,
                                CertificatePermissions.Update,
                                CertificatePermissions.Managecontacts,
                                CertificatePermissions.Getissuers,
                                CertificatePermissions.Listissuers,
                                CertificatePermissions.Setissuers,
                                CertificatePermissions.Deleteissuers,
                                CertificatePermissions.Manageissuers,
                                CertificatePermissions.Recover,
                                CertificatePermissions.Purge
                            },
                            Keys =
                            {
                                KeyPermissions.Encrypt,
                                KeyPermissions.Decrypt,
                                KeyPermissions.WrapKey,
                                KeyPermissions.UnwrapKey,
                                KeyPermissions.Sign,
                                KeyPermissions.Verify,
                                KeyPermissions.Get,
                                KeyPermissions.List,
                                KeyPermissions.Create,
                                KeyPermissions.Update,
                                KeyPermissions.Import,
                                KeyPermissions.Delete,
                                KeyPermissions.Backup,
                                KeyPermissions.Restore,
                                KeyPermissions.Recover,
                                KeyPermissions.Purge
                            },
                            Secrets =
                            {
                                SecretPermissions.Get,
                                SecretPermissions.List,
                                SecretPermissions.Set,
                                SecretPermissions.Delete,
                                SecretPermissions.Backup,
                                SecretPermissions.Restore,
                                SecretPermissions.Recover,
                                SecretPermissions.Purge
                            }
                        },
                        TenantId = identity.TenantId
                    },
                    new AccessPolicyEntryArgs
                    {
                        ObjectId = identity.PrincipalId,
                        Permissions = new PermissionsArgs
                        {
                            Certificates =
                            {
                                CertificatePermissions.Get,
                                CertificatePermissions.List,
                                CertificatePermissions.Delete,
                                CertificatePermissions.Create,
                                CertificatePermissions.Import,
                                CertificatePermissions.Update,
                                CertificatePermissions.Managecontacts,
                                CertificatePermissions.Getissuers,
                                CertificatePermissions.Listissuers,
                                CertificatePermissions.Setissuers,
                                CertificatePermissions.Deleteissuers,
                                CertificatePermissions.Manageissuers,
                                CertificatePermissions.Recover,
                                CertificatePermissions.Purge
                            },
                            Keys =
                            {
                                KeyPermissions.Encrypt,
                                KeyPermissions.Decrypt,
                                KeyPermissions.WrapKey,
                                KeyPermissions.UnwrapKey,
                                KeyPermissions.Sign,
                                KeyPermissions.Verify,
                                KeyPermissions.Get,
                                KeyPermissions.List,
                                KeyPermissions.Create,
                                KeyPermissions.Update,
                                KeyPermissions.Import,
                                KeyPermissions.Delete,
                                KeyPermissions.Backup,
                                KeyPermissions.Restore,
                                KeyPermissions.Recover,
                                KeyPermissions.Purge
                            },
                            Secrets =
                            {
                                SecretPermissions.Get,
                                SecretPermissions.List,
                                SecretPermissions.Set,
                                SecretPermissions.Delete,
                                SecretPermissions.Backup,
                                SecretPermissions.Restore,
                                SecretPermissions.Recover,
                                SecretPermissions.Purge
                            }
                        },
                        TenantId = identity.TenantId
                    }
                },
                EnabledForDeployment = true,
                EnabledForDiskEncryption = true,
                EnabledForTemplateDeployment = true,
                Sku = new VaultSkuArgs
                {
                    Family = SkuFamily.A,
                    Name = VaultSkuName.Standard
                },
                TenantId = identity.TenantId
            }
        });

        var keyResult = GetKey.Invoke(new()
        {
            ResourceGroupName = resourceGroup.Name,
            VaultName = vault.Name,
            KeyName = "opensjon"
        });

        var keyId = keyResult.Apply(_ => _.Id);

        var frontendIPConfigurationName = $"{application.Name}-feip";
        var frontendPortName = $"{application.Name}-feport";
        var frontendUnsecurePortName = $"{application.Name}-feport-unsecure";
        var applicationGatewayName = $"{application.Name}-appgw";
        var backendAddressPoolsName = $"{application.Name}-appgwpool";
        var backendHttpSettingsName = $"{application.Name}-appgwhbs";
        var httpListenerName = $"{application.Name}-appgwhl";
        var httpUnsecureListenerName = $"{application.Name}-appgwhl-unsecure";
        var sslCertificateName = $"{application.Name}-cert";
        var sslProfileName = $"{application.Name}-sslprofile";
        var resourceGroupId = resourceGroup.Id.Apply(id => id);

        var frontendIPConfigurationId = resourceGroupId.Apply(id =>
            $"{id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/frontendIPConfigurations/{frontendIPConfigurationName}");

        var frontendPortId = resourceGroupId.Apply(id =>
            $"{id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/frontendPorts/{frontendPortName}");

        var frontendUnsecurePortId = resourceGroupId.Apply(id =>
            $"{id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/frontendPorts/{frontendUnsecurePortName}");

        var backendAddressPoolsId = resourceGroupId.Apply(id =>
            $"{id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/backendAddressPools/{backendAddressPoolsName}");

        var backendHttpSettingsId = resourceGroupId.Apply(id =>
            $"{id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/backendHttpSettingsCollection/{backendHttpSettingsName}");

        var httpListenerId = resourceGroupId.Apply(id =>
            $"{id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/httpListeners/{httpListenerName}");

        var httpUnsecureListenerId = resourceGroupId.Apply(id =>
            $"{id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/httpListeners/{httpUnsecureListenerName}");

        var sslCertificateId = resourceGroupId.Apply(id =>
            $"{id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/sslCertificates/{sslCertificateName}");

        var sslProfileId = resourceGroupId.Apply(id =>
            $"{id}/providers/Microsoft.Network/applicationGateways/{applicationGatewayName}/sslProfiles/{sslProfileName}");

        var gateway = new ApplicationGateway(application.Name, new()
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
                    { "/subscriptions/d2139db2-c1bc-4480-84f1-7a4b1d97fcbb/resourceGroups/Einar-D-Norway-RG/providers/Microsoft.ManagedIdentity/userAssignedIdentities/MyApplication-usera3a1fa04", new Dictionary<string, object>() }
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
                    Name = backendAddressPoolsName,
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
                    Name = backendHttpSettingsName,
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
                    Name = "appgwipc",
                    Subnet = new SubResourceArgs { Id = virtualNetwork.Subnets.Apply(_ => _[2].Id!) }
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
                    Name = "appgwrule",
                    RuleType = ApplicationGatewayRequestRoutingRuleType.Basic,
                    Priority = 10,
                    BackendAddressPool = new SubResourceArgs { Id = backendAddressPoolsId },
                    BackendHttpSettings = new SubResourceArgs { Id = backendHttpSettingsId },
                    HttpListener = new SubResourceArgs { Id = httpListenerId },
                },
                new ApplicationGatewayRequestRoutingRuleArgs
                {
                    Name = "appgwrule-http",
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

        return new(virtualNetwork, profile, privateZone, publicIPAddress, null!);
    }
}
