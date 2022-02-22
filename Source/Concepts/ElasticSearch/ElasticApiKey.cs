// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.ElasticSearch;

public record ElasticApiKey(string Value) : ConceptAs<string>(Value)
{
    public static implicit operator ElasticApiKey(string value) => new(value);
}
