// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.ElasticSearch;

namespace Events.Organizations
{
    [EventType("39529780-02b3-49d7-8c4b-3489b7c7b59b")]
    public record ElasticSearchSettingsSet(ElasticUrl Url, ElasticApiKey ApiKey);
}
