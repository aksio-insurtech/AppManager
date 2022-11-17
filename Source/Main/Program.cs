// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Types;
using Sample;

Types.AddAssemblyPrefixesToExclude(
    "Pulumi",
    "Azure",
    "Google",
    "DnsClient",
    "CliWrap",
    "Ben.Demystifier",
    "Grpc");

var builder = Host.CreateDefaultBuilder()
                    .UseAksio(microserviceId: "39149c39-4fc7-4b93-ab6e-22b99ba12202")
                    .ConfigureWebHostDefaults(_ => _.UseStartup<Startup>());

var app = builder.Build();
app.Run();
