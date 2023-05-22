// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;
using Pulumi.AzureNative.ManagedIdentity;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

public static class ApplicationKeyVaultExtensions
{
    public static Vault SetupKeyVault(this Application application, ApplicationEnvironmentWithArtifacts environment, NetworkResult vnet, UserAssignedIdentity identity, ResourceGroup resourceGroup, Tags tags)
    {
        var name = $"{application.Name}-{environment.DisplayName}";
        return new Vault(
            application.Name.Value,
            new()
            {
                Location = environment.CloudLocation.Value,
                ResourceGroupName = resourceGroup.Name,
                Tags = tags,
                VaultName = name,
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
                    Sku = new SkuArgs
                    {
                        Family = SkuFamily.A,
                        Name = SkuName.Standard
                    },
                    TenantId = identity.TenantId,
                    NetworkAcls = new NetworkRuleSetArgs()
                    {
                        Bypass = NetworkRuleBypassOptions.AzureServices,
                        DefaultAction = NetworkRuleAction.Deny,
                        VirtualNetworkRules = new List<VirtualNetworkRuleArgs>()
                        {
                            new()
                            {
                                Id = vnet.GetInfrastructureSubnetId()
                            }
                        }
                    }
                }
            });
    }
}
