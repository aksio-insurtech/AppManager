// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications;

public record CloudLocationKey(string Value) : ConceptAs<string>(Value)
{
    public static readonly CloudLocationKey NorwayEast = "norwayeast";
    public static readonly CloudLocationKey NorwayWest = "norwaywest";
    public static readonly CloudLocationKey EuropeWest = "westeurope";
    public static readonly CloudLocationKey EuropeNorth = "northeurope";

    public static implicit operator CloudLocationKey(string value) => new(value);
}
