// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Events.Projections;
using Concepts.Applications;
using Events.Applications;
using ApplicationId = Concepts.Applications.ApplicationId;

namespace Read.Applications
{
    [Route("/api/cloudlocations")]
    public class CloudLocations : Controller
    {
        [HttpGet]
        public IEnumerable<CloudLocation> AllCloudLocations() => new[]
            {
                new CloudLocation("norway east", "Norway East"),
                new CloudLocation("westeurope", "West Europe"),
                new CloudLocation("northeurope", "North Europe")
            };
    }
}
