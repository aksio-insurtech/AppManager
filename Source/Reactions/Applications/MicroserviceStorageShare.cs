// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi;

namespace Reactions.Applications
{
    public record MicroserviceStorageShare(Input<string> AccountName, Input<string> AccountKey, Input<string> ShareName);
}
