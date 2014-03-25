// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Runtime
{
    using System.IO;
    using Autofac;
    using Narvalo.Narrative.Configuration;
    using Narvalo.Narrative.Weaving;

    public sealed class Runner
    {
        readonly Settings _settings;

        public Runner(Settings settings)
        {
            _settings = settings;
        }

        public void Run(string path, bool runInParallel)
        {
            var isDirectory = File.GetAttributes(path).HasFlag(FileAttributes.Directory);

            if (isDirectory) {
                ProcessDirectory_(new DirectoryInfo(path), runInParallel);
            }
            else {
                ProcessFile_(new FileInfo(path));
            }
        }

        void ProcessDirectory_(DirectoryInfo directory, bool runInParallel)
        {
            using (var container = CreateContainer_()) {
                container.Resolve<DirectoryWeaverFacade>().Process(directory, runInParallel);
            }
        }

        void ProcessFile_(FileInfo file)
        {
            using (var container = CreateContainer_()) {
                container.Resolve<IWeaver<FileInfo>>().Weave(file);
            }
        }

        IContainer CreateContainer_()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new WriterModule
            {
                DryRun = _settings.DryRun,
                OutputDirectory = _settings.OutputDirectory
            });

            builder.RegisterModule(new FileFinderModule());
            builder.RegisterModule(new WeaverEngineModule());
            builder.RegisterModule(new WeaverModule());

            return builder.Build();
        }
    }
}
