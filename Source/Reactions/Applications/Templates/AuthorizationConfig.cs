// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions.Applications.Templates;

/// <summary>
/// Represents the configuration for entra id roles requirement.
/// </summary>
/// <param name="ClientId">The client id for this identity provider.</param>
/// <param name="Roles">The required role(s).</param>
/// <param name="NoAuthorizationRequired">Can be set to true to ignore auth roles.</param>
public record AuthorizationConfig(string ClientId, IEnumerable<AuthorizationRole> Roles, bool NoAuthorizationRequired);