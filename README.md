# Aksio App Manager

[![C# Build](https://github.com/aksio-insurtech/Cratis/actions/workflows/dotnet-build.yml/badge.svg)](https://github.com/aksio-insurtech/Management/actions/workflows/dotnet-build.yml)
[![Web Build](https://github.com/aksio-insurtech/Cratis/actions/workflows/node-build.yml/badge.svg)](https://github.com/aksio-insurtech/Management/actions/workflows/web-build.yml)
[![Publish](https://github.com/aksio-insurtech/Cratis/actions/workflows/publish.yml/badge.svg)](https://github.com/aksio-insurtech/Management/actions/workflows/publish.yml)
[![Docker](https://img.shields.io/docker/v/aksioinsurtech/app-manager?label=Aksio%20App%20Manager&logo=docker&sort=semver)](https://hub.docker.com/r/aksioinsurtech/app-manager)

## Introduction

This repository holds tools and DevOps automation related to deploying and hosting solutions built on top of [Cratis](https://github.com/aksio-insurtech/Cratis).
It is an opinionated approach to how to host microservice solutions adhering to how Cratis wants things. Its target cloud is Microsoft Azure.

For general guidance on the core values and principles we @ Aksio use, read more [here](https://github.com/aksio-insurtech/Home/blob/main/profile/README.md).

If you want to jump into building this repository and possibly contributing, please refer to [contributing](./Documentation/contributing.md).

## Opening in VSCode online

If you prefer to browse the code in VSCode, you can do so by clicking [here](https://vscode.dev/github/aksio-insurtech/Cratis).

## Installing Pulumi plugins

Pulumi plugins have 2 parts to it; the NuGet package used as the C# API and then the actual plugin.
Within the [./Source/Reactions/Reactions.csproj](./Source/Reactions/Reactions.csproj) file you'll find the references for the NuGet
packages. In addition to this, we programmatically tell Pulumi to install the necessary plugins at runtime, which can be found inside
[./Source/Reactions/Applications/Pulumi/PulumiOperations.cs](./Source/Reactions/Applications/Pulumi/PulumiOperations.cs).

## Resources

Container Apps Vnet, follow this:
https://github.com/microsoft/azure-container-apps/issues/227

## Things to keep in mind

### MongoDB Atlas

#### API Keys

API keys are created by navigating to **Organization -> Access Manager -> API Keys**.
Backup configuration requires the IP address where Pulumi is running its automation from to be added to the access list of the API key.
This means that running AppManager in a cloud environment, the environments public IP address needs to be added to the API key.
Also, if you're testing from a local computer during development the public IP address for the site needs to be added as well.

## Tips & Tricks

To figure out what the public IP of something is:

```shell
curl api.ipify.org
```

If the Entra ID (AAD) service principal has changed, you currently need to manually set the passord in all the stacks.
First, export the stacks, manually change the passord from old to new and then import.

Below are the export and import commands for the current (as of 24.08.2023) stacks. You will need to log in with pulumi cli first, with an account that has the proper access (or use the pulumi PAT which can be found in the relevant variable.json file in the cloud-setup repo, under pulumi.accessToken).

### EXPORT
```sh
pulumi stack export --stack aksio/OPensjon/shared --show-secrets --file opensjon_shared_dev.yml
pulumi stack export --stack aksio/OPensjon-CaseWorkers/dev --show-secrets --file opensjon_caseworkers_dev.yml
pulumi stack export --stack aksio/OPensjon-Employees/dev --show-secrets --file opensjon_employees_dev.yml
pulumi stack export --stack aksio/OPensjon-Employers/dev --show-secrets --file opensjon_employers_dev.yml
pulumi stack export --stack aksio/OPensjon-FREG/dev --show-secrets --file opensjon_freg_dev.yml
pulumi stack export --stack aksio/OPensjon-KRR/dev --show-secrets --file opensjon_krr_dev.yml
pulumi stack export --stack aksio/OPensjon-Members/dev --show-secrets --file opensjon_members_dev.yml
pulumi stack export --stack aksio/OPensjon-NAV/dev --show-secrets --file opensjon_nav_dev.yml
pulumi stack export --stack aksio/OPensjon-NAVSoapProxy/dev --show-secrets --file opensjon_navsoapproxy_dev.yml
pulumi stack export --stack aksio/OPensjon-OFA/dev --show-secrets --file opensjon_ofa_dev.yml
pulumi stack export --stack aksio/OPensjon-SvarUT/dev --show-secrets --file opensjon_svarut_dev.yml
pulumi stack export --stack aksio/OPensjon/dev --show-secrets --file opensjon_shared_dev_dev.yml
pulumi stack export --stack aksio/OPensjon-CaseWorkers/qa --show-secrets --file opensjon_caseworkers_qa.yml
#pulumi stack export --stack aksio/OPensjon-Employees/qa --show-secrets --file opensjon_employees_qa.yml
pulumi stack export --stack aksio/OPensjon-Employers/qa --show-secrets --file opensjon_employers_qa.yml
pulumi stack export --stack aksio/OPensjon-FREG/qa --show-secrets --file opensjon_freg_qa.yml
pulumi stack export --stack aksio/OPensjon-KRR/qa --show-secrets --file opensjon_krr_qa.yml
pulumi stack export --stack aksio/OPensjon-Members/qa --show-secrets --file opensjon_members_qa.yml
pulumi stack export --stack aksio/OPensjon-NAV/qa --show-secrets --file opensjon_nav_qa.yml
pulumi stack export --stack aksio/OPensjon-NAVSoapProxy/qa --show-secrets --file opensjon_navsoapproxy_qa.yml
pulumi stack export --stack aksio/OPensjon-OFA/qa --show-secrets --file opensjon_ofa_qa.yml
pulumi stack export --stack aksio/OPensjon-SvarUT/qa --show-secrets --file opensjon_svarut_qa.yml
pulumi stack export --stack aksio/OPensjon/qa --show-secrets --file opensjon_shared_qa_qa.yml
pulumi stack export --stack aksio/OPensjon-CaseWorkers/prod --show-secrets --file opensjon_caseworkers_prod.yml
pulumi stack export --stack aksio/OPensjon-Employees/prod --show-secrets --file opensjon_employees_prod.yml
pulumi stack export --stack aksio/OPensjon-Employers/prod --show-secrets --file opensjon_employers_prod.yml
pulumi stack export --stack aksio/OPensjon-FREG/prod --show-secrets --file opensjon_freg_prod.yml
pulumi stack export --stack aksio/OPensjon-KRR/prod --show-secrets --file opensjon_krr_prod.yml
pulumi stack export --stack aksio/OPensjon-Members/prod --show-secrets --file opensjon_members_prod.yml
pulumi stack export --stack aksio/OPensjon-NAV/prod --show-secrets --file opensjon_nav_prod.yml
pulumi stack export --stack aksio/OPensjon-NAVSoapProxy/prod --show-secrets --file opensjon_navsoapproxy_prod.yml
pulumi stack export --stack aksio/OPensjon-OFA/prod --show-secrets --file opensjon_ofa_prod.yml
pulumi stack export --stack aksio/OPensjon-SvarUT/prod --show-secrets --file opensjon_svarut_prod.yml
pulumi stack export --stack aksio/OPensjon/prod --show-secrets --file opensjon_shared_prod_prod.yml
```

### IMPORT
```sh
pulumi stack import --stack aksio/OPensjon/shared --file opensjon_shared_dev.yml
pulumi stack import --stack aksio/OPensjon-CaseWorkers/dev --file opensjon_caseworkers_dev.yml
pulumi stack import --stack aksio/OPensjon-Employees/dev  --file opensjon_employees_dev.yml
pulumi stack import --stack aksio/OPensjon-Employers/dev --file opensjon_employers_dev.yml
pulumi stack import --stack aksio/OPensjon-FREG/dev  --file opensjon_freg_dev.yml
pulumi stack import --stack aksio/OPensjon-KRR/dev --file opensjon_krr_dev.yml
pulumi stack import --stack aksio/OPensjon-Members/dev --file opensjon_members_dev.yml
pulumi stack import --stack aksio/OPensjon-NAV/dev --file opensjon_nav_dev.yml
pulumi stack import --stack aksio/OPensjon-NAVSoapProxy/dev --file opensjon_navsoapproxy_dev.yml
pulumi stack import --stack aksio/OPensjon-OFA/dev  --file opensjon_ofa_dev.yml
pulumi stack import --stack aksio/OPensjon-SvarUT/dev  --file opensjon_svarut_dev.yml
pulumi stack import --stack aksio/OPensjon/dev --file opensjon_shared_dev_dev.yml
pulumi stack import --stack aksio/OPensjon-CaseWorkers/qa --file opensjon_caseworkers_qa.yml
#pulumi stack import --stack aksio/OPensjon-Employees/qa  --file opensjon_employees_qa.yml
pulumi stack import --stack aksio/OPensjon-Employers/qa --file opensjon_employers_qa.yml
pulumi stack import --stack aksio/OPensjon-FREG/qa  --file opensjon_freg_qa.yml
pulumi stack import --stack aksio/OPensjon-KRR/qa --file opensjon_krr_qa.yml
pulumi stack import --stack aksio/OPensjon-Members/qa --file opensjon_members_qa.yml
pulumi stack import --stack aksio/OPensjon-NAV/qa --file opensjon_nav_qa.yml
pulumi stack import --stack aksio/OPensjon-NAVSoapProxy/qa --file opensjon_navsoapproxy_qa.yml
pulumi stack import --stack aksio/OPensjon-OFA/qa  --file opensjon_ofa_qa.yml
pulumi stack import --stack aksio/OPensjon-SvarUT/qa  --file opensjon_svarut_qa.yml
pulumi stack import --stack aksio/OPensjon/qa --file opensjon_shared_qa_qa.yml
pulumi stack import --stack aksio/OPensjon-CaseWorkers/prod --file opensjon_caseworkers_prod.yml
pulumi stack import --stack aksio/OPensjon-Employees/prod  --file opensjon_employees_prod.yml
pulumi stack import --stack aksio/OPensjon-Employers/prod --file opensjon_employers_prod.yml
pulumi stack import --stack aksio/OPensjon-FREG/prod  --file opensjon_freg_prod.yml
pulumi stack import --stack aksio/OPensjon-KRR/prod --file opensjon_krr_prod.yml
pulumi stack import --stack aksio/OPensjon-Members/prod --file opensjon_members_prod.yml
pulumi stack import --stack aksio/OPensjon-NAV/prod --file opensjon_nav_prod.yml
pulumi stack import --stack aksio/OPensjon-NAVSoapProxy/prod --file opensjon_navsoapproxy_prod.yml
pulumi stack import --stack aksio/OPensjon-OFA/prod  --file opensjon_ofa_prod.yml
pulumi stack import --stack aksio/OPensjon-SvarUT/prod  --file opensjon_svarut_prod.yml
pulumi stack import --stack aksio/OPensjon/prod --file opensjon_shared_prod_prod.yml
```
