// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.App;

namespace Reactions.Applications.Pulumi;
#pragma warning disable RCS1175, IDE0059

public record IngressResult(string Url, string FileShareId, ContainerApp ContainerApp);
