// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.IO;
    using Autofac;
    using Narvalo.Narrative.Configuration;
    using Narvalo.Narrative.Processors;

    public sealed class ProgramFacade
    {
        readonly Settings _settings;
        IContainer _container;

        public ProgramFacade(Settings settings, IContainer container)
        {
            _settings = settings;
            _container = container;
        }

        public void Process(string path)
        {
            var isDirectory = File.GetAttributes(path).HasFlag(FileAttributes.Directory);

            if (isDirectory) {
                var directory = new DirectoryInfo(path);

                if (_settings.RunInParallel) {
                    _container.Resolve<ParallelDirectoryProcessor>().Process(directory);
                }
                else {
                    _container.Resolve<SequentialDirectoryProcessor>().Process(directory);
                }
            }
            else {
                _container.Resolve<FileProcessor>().Process(new FileInfo(path));
            }
        }
    }
}
