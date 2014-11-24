// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Narrator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Autofac;
    using Narvalo.IO;
    using Narvalo.Narrative.Internal;
    using Narvalo.Narrative.Weaving;

    public sealed class WeaverModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Require.NotNull(builder, "builder");

            // FIXME: Automation for interception?
            builder.RegisterType<FileWeaverInterceptor>().AsSelf();
            builder.RegisterType<RelativeFileWeaverInterceptor>().AsSelf();

            // Absolute file weaver.
            builder.RegisterType<FileWeaver>()
                .As<IWeaver<FileInfo>>()
                .InterfaceDebuggedBy(typeof(FileWeaverInterceptor));

            // Relative file weaver (used by DirectoryWeaver).
            builder.RegisterType<RelativeFileWeaver>()
                .As<IWeaver<RelativeFile>>()
                .InterfaceDebuggedBy(typeof(RelativeFileWeaverInterceptor));

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
