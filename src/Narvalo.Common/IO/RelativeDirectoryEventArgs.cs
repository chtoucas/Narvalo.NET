// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;
    using System.Diagnostics.Contracts;

    public sealed class RelativeDirectoryEventArgs : EventArgs
    {
        private readonly RelativeDirectory _relativeDirectory;

        public RelativeDirectoryEventArgs(RelativeDirectory relativeDirectory)
        {
            Require.NotNull(relativeDirectory, nameof(relativeDirectory));

            _relativeDirectory = relativeDirectory;
        }

        public RelativeDirectory RelativeDirectory
        {
            get
            {
                Warrant.NotNull<RelativeDirectory>();

                return _relativeDirectory;
            }
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_relativeDirectory != null);
        }

#endif
    }
}
