// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.IO;
    using System.Threading.Tasks;
    using Narvalo.IO;

    public sealed class ParallelRunner : DirectoryRunnerBase
    {
        public ParallelRunner(IWeaver weaver, DirectoryInfo directory, string outputDirectory)
            : base(weaver, directory, outputDirectory) { }

        public override void Run()
        {
            var finder = new ConcurrentFileFinder(DirectoryFilter, FileFilter);
            finder.DirectoryStart += OnDirectoryStart;

            var sources = finder.Find(Directory, "*.cs");

            Parallel.ForEach(sources, RunCore);
        }
    }
}
