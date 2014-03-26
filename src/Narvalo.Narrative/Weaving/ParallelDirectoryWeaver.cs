// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Weaving
{
    using System.IO;
    using System.Threading.Tasks;
    using Narvalo.IO;

    public sealed class ParallelDirectoryWeaver : IWeaver<DirectoryInfo>
    {
        readonly ConcurrentFileFinder _finder;
        readonly IWeaver<RelativeFile> _processor;

        public ParallelDirectoryWeaver(IWeaver<RelativeFile> processor, ConcurrentFileFinder finder)
        {
            Require.NotNull(processor, "processor");
            Require.NotNull(finder, "finder");

            _processor = processor;
            _finder = finder;
        }

        public void Weave(DirectoryInfo directory)
        {
            var sources = _finder.Find(directory, "*.cs");

            Parallel.ForEach(sources, _processor.Weave);
        }
    }
}
