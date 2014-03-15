// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;

    public sealed class RelativeDirectoryEventArgs : EventArgs
    {
        readonly RelativeDirectory _relativeDirectory;

        public RelativeDirectoryEventArgs(RelativeDirectory relativeDirectory)
        {
            Require.NotNull(relativeDirectory, "relativeDirectory");

            _relativeDirectory = relativeDirectory;
        }

        public RelativeDirectory RelativeDirectory
        {
            get { return _relativeDirectory; }
        }
    }
}
