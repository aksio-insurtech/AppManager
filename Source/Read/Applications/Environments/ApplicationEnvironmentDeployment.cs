// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Read.Applications.Environments;

public record ApplicationEnvironmentDeployment(
    ApplicationEnvironmentDeploymentKey Id,
    DateTimeOffset Started,
    DateTimeOffset CompletedOrFailed,
    ApplicationEnvironmentDeploymentStatus Status,
    IEnumerable<string> Errors,
    string StackTrace,
    string Log);
