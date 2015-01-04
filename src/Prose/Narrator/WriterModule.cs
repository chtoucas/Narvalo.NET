// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose.Narrator
{
    using Autofac;
    using Narvalo;
    using Prose.IO;

    public sealed class WriterModule : Module
    {
        public bool DryRun { get; set; }

        public string OutputDirectory { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            Require.NotNull(builder, "builder");

            builder.Register(_ => new PathProvider(OutputDirectory)).As<IPathProvider>();

            if (DryRun) {
                builder.RegisterType<NoopWriter>().As<IOutputWriter>().InstancePerLifetimeScope();
            }
            else {
                builder.RegisterType<OutputWriter>().As<IOutputWriter>().InstancePerLifetimeScope();
            }
        }
    }
}
