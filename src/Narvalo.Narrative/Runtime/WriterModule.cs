// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Runtime
{
    using Autofac;
    using Narvalo.Narrative.Configuration;

    public sealed class WriterModule : Module
    {
        public Settings Settings { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => new PathProvider(Settings.OutputDirectory)).As<IPathProvider>();

            if (Settings.DryRun) {
                builder.RegisterType<NoopWriter>().As<IOutputWriter>();
            }
            else {
                builder.RegisterType<OutputWriter>().As<IOutputWriter>();
            }
        }
    }
}
