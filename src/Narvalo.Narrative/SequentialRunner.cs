// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.IO;
    using Narvalo.IO;

    public sealed class SequentialRunner : DirectoryRunnerBase
    {
        public SequentialRunner(IWeaver weaver, DirectoryInfo directory, string outputDirectory)
            : base(weaver, directory, outputDirectory) { }

        public override void Run()
        {
            var finder = new FileFinder(DirectoryFilter, FileFilter);
            finder.DirectoryStart += OnDirectoryStart;

            var sources = finder.Find(Directory, "*.cs");

            foreach (var source in sources) {
                RunCore(source);
            };
        }
    }
}
