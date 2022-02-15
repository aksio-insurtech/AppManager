// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Common;

namespace Read.Organizations
{
    public class SettingsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var collection = context.Resolve<IMongoCollection<Settings>>();
                return collection.Find(_ => true).FirstOrDefault() ?? Settings.NoSettings;
            }).As<ISettings>();
        }
    }
}
