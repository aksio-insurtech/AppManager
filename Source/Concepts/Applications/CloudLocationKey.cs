// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications;

public record CloudLocationKey(string Value) : ConceptAs<string>(Value)
{
    public const string NorwayEast = "norwayeast";
    public const string EuropeWest = "westeurope";
    public const string EuropeNorth = "northeurope";

    public static implicit operator CloudLocationKey(string value) => new(value);
}
