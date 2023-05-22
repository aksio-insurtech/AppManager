// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Bootstrap;

public class EnvironmentStorageAccessListEntryDoesNotHaveAnIpAddress : Exception
{
    public EnvironmentStorageAccessListEntryDoesNotHaveAnIpAddress()
        : base("An environment storage accesslist entry is defined, but does not have a valid IP address")
    {
    }
}