// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions.Applications.Templates;

/// <summary>
/// Ingress handlebar template parameters.
/// </summary>
/// <param name="Path">Location path.</param>
/// <param name="TargetUrl">Target path.</param>
/// <param name="Resolver">If path is a regular expression, you need to specify a (dns) resolver. For eaxmple google's 8.8.8.8.</param>
public record IngressTemplateRouteContent(string Path, string TargetUrl, string? Resolver);
