// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.ElasticSearch;

namespace Bootstrap;

public record ElasticSearchConfig(ElasticUrl Url, ElasticApiKey ApiKey);
