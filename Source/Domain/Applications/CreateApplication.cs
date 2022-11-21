// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Domain.Applications;

public record CreateApplication(ApplicationId ApplicationId, ApplicationName Name);
