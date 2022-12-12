// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Resources;
using Reactions.Applications.Pulumi.Resources;

namespace Reactions.Applications;

public record Resource(ResourceId Id, ResourceTypeId Type, ResourceName Name, IResourceConfiguration Configuration);
