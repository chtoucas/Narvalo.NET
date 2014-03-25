// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Weaving
{
    using System.IO;
    using Narvalo.IO;

    public sealed class SequentialDirectoryWeaver : IWeaver<DirectoryInfo>
    {
        readonly IWeaver<RelativeFile> _processor;
        readonly FileFinder _finder;

        public SequentialDirectoryWeaver(IWeaver<RelativeFile> processor, FileFinder finder)
        {
            Require.NotNull(processor, "processor");
            Require.NotNull(finder, "finder");

            _processor = processor;
            _finder = finder;
        }

        public void Weave(DirectoryInfo directory)
        {
            var sources = _finder.Find(directory, "*.cs");

            foreach (var source in sources) {
                _processor.Weave(source);
            };
        }
    }
}
