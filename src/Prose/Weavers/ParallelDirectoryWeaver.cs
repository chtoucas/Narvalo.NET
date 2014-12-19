// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose.Weavers
{
    using System.IO;
    using System.Threading.Tasks;
    using Narvalo;
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

        public void Weave(DirectoryInfo source)
        {
            var sources = _finder.Find(source, "*.cs");

            Parallel.ForEach(sources, _processor.Weave);
        }
    }
}
