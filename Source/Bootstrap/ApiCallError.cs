// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Bootstrap;

public class ApiCallError : Exception
{
    public ApiCallError(string url) : base($"Error when calling API at '{url}'")
    {
    }
}
