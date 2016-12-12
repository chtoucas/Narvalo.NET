// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System.IO;

    public sealed class RelativeDirectory
    {
        private readonly DirectoryInfo _directory;
        private readonly string _relativeName;

        // REVIEW: Require.NotEmpty on relativeName?
        public RelativeDirectory(DirectoryInfo directory, string relativeName)
        {
            Require.NotNull(directory, nameof(directory));
            Require.NotNull(relativeName, nameof(relativeName));

            _directory = directory;
            _relativeName = relativeName;
        }

        public DirectoryInfo Directory
        {
            get
            {
                Warrant.NotNull<DirectoryInfo>();

                return _directory;
            }
        }

        public string RelativeName
        {
            get
            {
                Warrant.NotNull<string>();

                return _relativeName;
            }
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_directory != null);
            Contract.Invariant(_relativeName != null);
        }

#endif
    }
}
