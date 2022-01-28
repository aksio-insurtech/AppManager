// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Events.Projections;
using Concepts.Applications;
using Events.Applications;
using ApplicationId = Concepts.Applications.ApplicationId;

namespace Read.Applications
{
    public record Application(ApplicationId Id, ApplicationName Name, CloudLocationKey CloudLocation);
}
