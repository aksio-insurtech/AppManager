// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;
using Concepts.ElasticSearch;

namespace Reactions.Applications
{
    public record AppSettingsValues(ApplicationName ApplicationName, ElasticUrl ElasticUrl, ElasticApiKey ElasticApiKey);
}