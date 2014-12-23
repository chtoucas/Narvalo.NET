// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose.Narrator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Narvalo;
    using Prose.Weavers;

    public sealed class DirectoryWeaverFacade
    {
        readonly IEnumerable<Lazy<IWeaver<DirectoryInfo>, ParallelExecutionMetadata>> _weavers;

        public DirectoryWeaverFacade(
            IEnumerable<Lazy<IWeaver<DirectoryInfo>, ParallelExecutionMetadata>> weavers)
        {
            Require.NotNull(weavers, "weavers");

            _weavers = weavers;
        }

        public void Process(DirectoryInfo directory, bool runInParallel)
        {
            Require.NotNull(directory, "directory");

            var processor = _weavers.Single(e => e.Metadata.RunInParallel == runInParallel);

            processor.Value.Weave(directory);
        }
    }
}
