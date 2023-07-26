// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Sample;

var builder = Host.CreateDefaultBuilder()
                    .UseMongoDB()
                    .UseAksio()
                    .UseCratis(_ => _.ForMicroservice("8c538618-2862-4018-b29d-17a4ec131958", "Management"))
                    .ConfigureWebHostDefaults(_ => _.UseStartup<Startup>());

var app = builder.Build();
app.Run();
