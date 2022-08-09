// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Bootstrap;

public class InMemoryEventLog : IEventLog
{
    public Task Append(EventSourceId eventSourceId, object @event, DateTimeOffset? validFrom = null) => Task.CompletedTask;
}
