// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Runtime
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
            // FIXME: Automation for interception.
            builder.RegisterType<FileSystemInfoInterceptor>().AsSelf();
            builder.RegisterType<RelativeFileInterceptor>().AsSelf();

            builder.RegisterType<RelativeFileWeaver>()
                .As<IWeaver<RelativeFile>>()
                .InterfaceDebuggedBy(typeof(RelativeFileInterceptor));

            builder.RegisterType<FileWeaver>()
                .As<IWeaver<FileInfo>>()
                .InterfaceDebuggedBy(typeof(FileSystemInfoInterceptor));

            builder.RegisterType<SequentialDirectoryWeaver>()
                .As<IWeaver<DirectoryInfo>>()
                .WithMetadata<ParallelExecutionMetadata>(_ => _.For(pe => pe.RunInParallel, false));

            builder.RegisterType<ParallelDirectoryWeaver>()
                .As<IWeaver<DirectoryInfo>>()
                .WithMetadata<ParallelExecutionMetadata>(_ => _.For(pe => pe.RunInParallel, true));

            builder.Register(c => new DirectoryWeaverFacade(
                c.Resolve<IEnumerable<Lazy<IWeaver<DirectoryInfo>, ParallelExecutionMetadata>>>()));
        }
    }
}
