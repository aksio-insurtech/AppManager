# Aksio Cloud Management tool

[![C# Build](https://github.com/aksio-insurtech/Cratis/actions/workflows/dotnet-build.yml/badge.svg)](https://github.com/aksio-insurtech/Management/actions/workflows/dotnet-build.yml)
[![Web Build](https://github.com/aksio-insurtech/Cratis/actions/workflows/node-build.yml/badge.svg)](https://github.com/aksio-insurtech/Management/actions/workflows/web-build.yml)
[![Publish](https://github.com/aksio-insurtech/Cratis/actions/workflows/publish.yml/badge.svg)](https://github.com/aksio-insurtech/Management/actions/workflows/publish.yml)
[![Docker](https://img.shields.io/docker/v/aksioinsurtech/app-manager?label=Aksio%20Cloud%20Management&logo=docker&sort=semver)](https://hub.docker.com/r/aksioinsurtech/app-manager)

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

https://github.com/pulumi/examples/blob/master/azure-ts-static-website/index.ts

https://www.pulumi.com/registry/packages/azure-native/api-docs/containerinstance/containergroup/

https://docs.microsoft.com/en-us/azure/container-instances/container-instances-volume-azure-files
