// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Reactions.Applications;

/// <summary>
/// Ingress route parameters.
/// </summary>
/// <param name="Path">Location path.</param>
/// <param name="TargetMicroservice">Target microservice id.</param>
/// <param name="TargetPath">Target path.</param>
/// <param name="UseResolver">Set true if the path utilizes a regexp and thus needs to use the ingress resolver.</param>
public record Route(string Path, MicroserviceId TargetMicroservice, string TargetPath, bool UseResolver = false);
