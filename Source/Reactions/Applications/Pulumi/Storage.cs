// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions.Applications.Pulumi;

public record Storage(string AccountName, string AccountKey)
{
    public string ConnectionString => $"DefaultEndpointsProtocol=https;AccountName={AccountName};AccountKey={AccountKey};EndpointSuffix=core.windows.net";
}
