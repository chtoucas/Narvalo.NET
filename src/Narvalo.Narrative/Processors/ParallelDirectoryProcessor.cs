// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Processors
{
    using System.IO;
    using System.Threading.Tasks;
    using Narvalo.IO;

    public sealed class ParallelDirectoryProcessor : DirectoryProcessorBase
    {
        public ParallelDirectoryProcessor(IWeaver weaver, IOutputWriter writer)
            : base(weaver, writer) { }

        public override void Process(DirectoryInfo directory)
        {
            var finder = new ConcurrentFileFinder(DirectoryFilter, FileFilter);
            finder.DirectoryStart += OnDirectoryStart;

            var sources = finder.Find(directory, "*.cs");

            Parallel.ForEach(sources, ProcessFile);
        }
    }
}
