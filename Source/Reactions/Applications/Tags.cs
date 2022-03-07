// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions.Applications;

public class Tags : Dictionary<string, string>
{
    public Tags(IDictionary<string, string> tags) : base(tags)
    {
    }

    public Tags Clone()
    {
        return new(this.ToDictionary(_ => _.Key, _ => _.Value));
    }
}
