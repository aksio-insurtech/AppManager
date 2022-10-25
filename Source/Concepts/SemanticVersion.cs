// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Concepts;

public record SemanticVersion(int Major, int Minor, int Patch, string PreRelease = "", string Metadata = "")
{
    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder
            .Append(Major)
            .Append('.')
            .Append(Minor)
            .Append('.')
            .Append(Patch);

        if (!string.IsNullOrEmpty(PreRelease))
        {
            stringBuilder
                .Append('-')
                .Append(PreRelease);
        }

        if (!string.IsNullOrEmpty(Metadata))
        {
            stringBuilder
                .Append('.')
                .Append(Metadata);
        }

        return stringBuilder.ToString();
    }
}
