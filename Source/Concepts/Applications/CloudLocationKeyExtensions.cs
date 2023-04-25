// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications;

public static class CloudLocationKeyExtensions
{
    public static string ToAtlas(this CloudLocationKey cloudLocation)
    {
        if (cloudLocation.Value == CloudLocationKey.NorwayEast) return "NORWAY_EAST";
        if (cloudLocation.Value == CloudLocationKey.NorwayWest) return "NORWAY_WEST";
        if (cloudLocation.Value == CloudLocationKey.EuropeNorth) return "EUROPE_NORTH";
        if (cloudLocation.Value == CloudLocationKey.EuropeWest) return "EUROPE_WEST";
        return string.Empty;
    }

    public static string ToCountryName(this CloudLocationKey cloudLocation)
    {
        if (cloudLocation.Value == CloudLocationKey.NorwayEast) return "Norway";
        if (cloudLocation.Value == CloudLocationKey.NorwayWest) return "Norway";
        if (cloudLocation.Value == CloudLocationKey.EuropeNorth) return "Netherlands";
        if (cloudLocation.Value == CloudLocationKey.EuropeWest) return "Ireland";
        return "NA";
    }
}
