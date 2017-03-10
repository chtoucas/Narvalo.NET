// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;

    public sealed partial class RelativeDirectoryEventArgs : EventArgs
    {
        public RelativeDirectoryEventArgs(RelativeDirectory relativeDirectory)
        {
            Require.NotNull(relativeDirectory, nameof(relativeDirectory));

            RelativeDirectory = relativeDirectory;
        }

        public RelativeDirectory RelativeDirectory { get; }
    }
}
