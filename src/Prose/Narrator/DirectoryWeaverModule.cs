// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose.Narrator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Autofac;
    using Prose.Weavers;
    using Narvalo;

    public sealed class DirectoryWeaverModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Require.NotNull(builder, "builder");

            // Sequential directory weaver.
            builder.RegisterType<SequentialDirectoryWeaver>()
                .As<IWeaver<DirectoryInfo>>()
                .WithMetadata<ParallelExecutionMetadata>(_ => _.For(pem => pem.RunInParallel, false));

            // Parallel directory weaver.
            builder.RegisterType<ParallelDirectoryWeaver>()
                .As<IWeaver<DirectoryInfo>>()
                .WithMetadata<ParallelExecutionMetadata>(_ => _.For(pem => pem.RunInParallel, true));

            // Facade for the directory weavers.
            builder.Register(c => new DirectoryWeaverFacade(
                c.Resolve<IEnumerable<Lazy<IWeaver<DirectoryInfo>, ParallelExecutionMetadata>>>()));
        }
    }
}
