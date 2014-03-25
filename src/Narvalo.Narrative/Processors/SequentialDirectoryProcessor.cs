// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Processors
{
    using System.IO;
    using Narvalo.IO;

    public sealed class SequentialDirectoryProcessor : DirectoryProcessorBase
    {
        public SequentialDirectoryProcessor(IWeaver weaver, IOutputWriter writer)
            : base(weaver, writer) { }

        public override void Process(DirectoryInfo directory)
        {
            var finder = new FileFinder(DirectoryFilter, FileFilter);
            finder.DirectoryStart += OnDirectoryStart;

            var sources = finder.Find(directory, "*.cs");

            foreach (var source in sources) {
                ProcessFile(source);
            };
        }
    }
}
