// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Reactions.Applications;

namespace Bootstrap;

public record ApplicationAndEnvironment(ApplicationId Id, ApplicationName Name, ApplicationEnvironmentWithArtifacts Environment);