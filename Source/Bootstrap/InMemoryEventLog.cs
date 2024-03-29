// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Bootstrap;

public class InMemoryEventLog : IEventLog
{
    public Task Append(EventSourceId eventSourceId, object @event, DateTimeOffset? validFrom = null) => Task.CompletedTask;
    public Task<EventSequenceNumber> GetNextSequenceNumber() => throw new NotImplementedException();
    public Task<EventSequenceNumber> GetTailSequenceNumber() => throw new NotImplementedException();
    public Task<EventSequenceNumber> GetTailSequenceNumberForObserver(Type type) => throw new NotImplementedException();
    public Task Redact(EventSequenceNumber sequenceNumber, RedactionReason? reason = null) => throw new NotImplementedException();
    public Task Redact(EventSourceId eventSourceId, RedactionReason? reason = null, params Type[] eventTypes) => throw new NotImplementedException();
}
