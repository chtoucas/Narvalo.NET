// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System.Diagnostics.Contracts;
    using System.IO;

    public sealed class RelativeDirectory
    {
        readonly DirectoryInfo _directory;
        readonly string _relativeName;

        // REVIEW: Require.NotEmpty on relativeName?
        public RelativeDirectory(DirectoryInfo directory, string relativeName)
        {
            Require.NotNull(directory, "directory");
            Require.NotNull(relativeName, "relativeName");

            _directory = directory;
            _relativeName = relativeName;
        }

        public DirectoryInfo Directory
        {
            get
            {
                Contract.Ensures(Contract.Result<DirectoryInfo>() != null);
                return _directory;
            }
        }

        public string RelativeName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return _relativeName;
            }
        }

#if CONTRACTS_FULL
        [ContractInvariantMethod]
        void ObjectInvariants()
        {
            Contract.Invariant(_directory != null);
            Contract.Invariant(_relativeName != null);
        }
#endif
    }
}
