// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Bootstrap;
#pragma warning disable AS0006

public class InvalidAuthentication : Exception
{
    public InvalidAuthentication(string message)
        : base(message)
    {
    }
}