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

        // Aggregate all domains for dupe-check before we configure.
        var domains = new List<Domain>();

        // Set up the ingress domain certificates.
        // If you get an ingress setup error stating that the manged certificate cannot be configured since no container app listens to it, the current procedure is this:
        //  #1: in your environment json file (e.g. "OPensjon.json"), for the ingress in question: do not define the "certificateId" value and then run.
        //  #2: after the one run, you can set the certificateId value and run this again to get it properly set up.
        // Yes, it kind of sucks. But until we rewrite this it is what it is.
        domains.AddRange(environment.Ingresses.Select(i => i.Domain).Where(d => d?.CertificateId != null)!);

        // Set up the auth domain certificates.
        domains.AddRange(environment.Ingresses.Select(i => i.AuthDomain).Where(d => d != null)!);

        // Identityprovider domain certificates
        domains.AddRange(environment.Tenants.SelectMany(t => t.IdentityProviders.Select(ip => ip.Domain)).Where(d => d != null)!);
        var dupes = domains.GroupBy(c => c.CertificateId).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        if (dupes.Any())
        {
            throw new DuplicateCertificateIdsUsedInConfiguration(dupes);
        }

        foreach (var domain in domains)
        {
            var certificateResult = new ManagedCertificate(
                domain!.Name,
                new ManagedCertificateArgs()
                {
                    ResourceGroupName = resourceGroup.Name,
                    Tags = tags,
                    EnvironmentName = managedEnvironment.Name,
                    Properties = new ManagedCertificatePropertiesArgs()
                    {
                        SubjectName = domain.Name.Value,
                        DomainControlValidation = ManagedCertificateDomainControlValidation.HTTP
                    }
                });
            certificateResourceIdentifiers[domain.CertificateId] = certificateResult.Id;
        }

        return certificateResourceIdentifiers;
    }
}