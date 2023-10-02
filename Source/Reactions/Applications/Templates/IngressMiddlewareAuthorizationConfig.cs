// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions.Applications.Templates;

/// <summary>
/// Represents the configuration for entra id roles requirement, but with plain roles as opposed to role with type.
/// </summary>
/// <param name="ClientId">The client id for this identity provider.</param>
/// <param name="Roles">The required role(s).</param>
/// <param name="NoAuthorizationRequired">Can be set to true to ignore auth roles.</param>
public record IngressMiddlewareAuthorizationConfig(string ClientId, IEnumerable<string> Roles, bool NoAuthorizationRequired);