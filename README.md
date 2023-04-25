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
