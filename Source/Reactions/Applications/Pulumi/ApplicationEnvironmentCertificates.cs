// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;
using Pulumi;
using Pulumi.AzureNative.App;
using Pulumi.AzureNative.App.Inputs;
using Pulumi.AzureNative.Resources;

namespace Reactions.Applications.Pulumi;

#pragma warning disable RCS1175

public static class ApplicationEnvironmentCertificates
{
    public static IDictionary<CertificateId, Output<string>> SetupCertificates(
        this Application application,
        ApplicationEnvironmentWithArtifacts environment,
        ManagedEnvironment managedEnvironment,
        ResourceGroup resourceGroup,
        Tags tags)
    {
        var certificateResourceIdentifiers = new Dictionary<CertificateId, Output<string>>();
        foreach (var certificate in environment.Certificates)
        {
            var certificateResult = new global::Pulumi.AzureNative.App.Certificate(certificate.Name, new()
            {
                ResourceGroupName = resourceGroup.Name,
                Tags = tags,
                EnvironmentName = managedEnvironment.Name,
                Properties = new CertificatePropertiesArgs()
                {
                    Value = certificate.Value.Value,
                    Password = certificate.Password.Value
                }
            });
            certificateResourceIdentifiers[certificate.Id] = certificateResult.Id;
        }

        return certificateResourceIdentifiers;
    }
}
