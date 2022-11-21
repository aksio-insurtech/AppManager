// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Concepts;

public class SemanticVersionJsonConverter : JsonConverter<SemanticVersion>
{
    static readonly Regex _regularExpression = new(@"(?<Major>0|(?:[1-9]\d*))(?:\.(?<Minor>0|(?:[1-9]\d*))(?:\.(?<Patch>0|(?:[1-9]\d*)))?(?:\-(?<PreRelease>[0-9A-Z\.-]+))?(?:\+(?<Meta>[0-9A-Z\.-]+))?)?", RegexOptions.Compiled | RegexOptions.ExplicitCapture, TimeSpan.FromSeconds(1));

    public override SemanticVersion? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var version = reader.GetString()!;
        var match = _regularExpression.Match(version);
        return new SemanticVersion(
            int.Parse(match.Groups["Major"].Value),
            int.Parse(match.Groups["Minor"].Value),
            int.Parse(match.Groups["Patch"].Value),
            match.Groups["PreRelease"].Value,
            match.Groups["Meta"].Value);
    }

    public override void Write(Utf8JsonWriter writer, SemanticVersion value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
