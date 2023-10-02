// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions.Applications;

/// <summary>
/// A required role to be authorized through the ingress.
/// </summary>
/// <param name="Value">The role value.</param>
public record AuthorizedRole(string Value) : ConceptAs<string>(Value);