// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions.Applications.Templates;

/// <summary>
/// The authorization role types.
/// This is needed because audience in the principal/token is different in user login and app login.
/// </summary>
public enum AuthorizationRoleTypes
{
    Undefined = 0,
    User = 1,
    App = 2
}