﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;
    using System.Diagnostics.Contracts;

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
            get
            {
                Contract.Ensures(Contract.Result<RelativeDirectory>() != null);
                return _relativeDirectory;
            }
        }

#if CONTRACTS_FULL
        [ContractInvariantMethod]
        void ObjectInvariants()
        {
            Contract.Invariant(_relativeDirectory != null);
        }
#endif
    }
}
