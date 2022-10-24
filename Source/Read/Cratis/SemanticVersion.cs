// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Read.Cratis;

public record SemanticVersion(int Major, int Minor, int Patch, string PreRelease, string Metadata);
