// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Types;
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
                    .UseAksio(microserviceId: "8c538618-2862-4018-b29d-17a4ec131958")
                    .ConfigureWebHostDefaults(_ => _.UseStartup<Startup>());

var app = builder.Build();
app.Run();
