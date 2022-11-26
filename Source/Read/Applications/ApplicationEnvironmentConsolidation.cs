// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Read.Applications;

public record ApplicationEnvironmentConsolidation(
    ApplicationEnvironmentConsolidationKey Id,
    DateTimeOffset Started,
    DateTimeOffset CompletedOrFailed,
    ApplicationEnvironmentConsolidationStatus Status,
    string Log);