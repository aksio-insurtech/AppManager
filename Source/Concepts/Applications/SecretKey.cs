// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Concepts.Applications;

public record SecretKey(string Key) : ConceptAs<string>(Key)
{
    public static implicit operator SecretKey(string key) => new(key);
}
