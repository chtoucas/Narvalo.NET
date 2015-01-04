// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose.Weavers
{
    using System.IO;
    using Narvalo;
    using Narvalo.IO;

    public sealed class SequentialDirectoryWeaver : IWeaver<DirectoryInfo>
    {
        readonly FileFinder _finder;
        readonly IWeaver<RelativeFile> _processor;

        public SequentialDirectoryWeaver(IWeaver<RelativeFile> processor, FileFinder finder)
        {
            Require.NotNull(processor, "processor");
            Require.NotNull(finder, "finder");

            _processor = processor;
            _finder = finder;
        }

        public void Weave(DirectoryInfo source)
        {
            var sources = _finder.Find(source, "*.cs");

            foreach (var item in sources) {
                _processor.Weave(item);
            }
        }
    }
}
