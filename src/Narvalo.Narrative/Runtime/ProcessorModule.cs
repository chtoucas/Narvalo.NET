// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Runtime
{
    using Autofac;
    using Narvalo.Narrative.Processors;

    public sealed class ProcessorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FileProcessor>().AsSelf();
            builder.RegisterType<SequentialDirectoryProcessor>().AsSelf();
            builder.RegisterType<ParallelDirectoryProcessor>().AsSelf();
        }
    }
}
