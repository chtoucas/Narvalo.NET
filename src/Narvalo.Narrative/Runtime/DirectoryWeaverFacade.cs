// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Narvalo.Narrative.Weaving;

    public sealed class DirectoryWeaverFacade
    {
        readonly IEnumerable<Lazy<IWeaver<DirectoryInfo>, ParallelExecutionMetadata>> _processors;

        public DirectoryWeaverFacade(
            IEnumerable<Lazy<IWeaver<DirectoryInfo>, ParallelExecutionMetadata>> processors)
        {
            _processors = processors;
        }

        public void Process(DirectoryInfo directory, bool runInParallel)
        {
            var processor = _processors.Single(e => e.Metadata.RunInParallel == runInParallel);

            processor.Value.Weave(directory);
        }
    }
}
