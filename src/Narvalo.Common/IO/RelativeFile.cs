// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System.Diagnostics.Contracts;
    using System.IO;

    public sealed class RelativeFile
    {
        private readonly FileInfo _file;
        private readonly string _relativeDirectoryName;

        // REVIEW: Require.NotEmpty on relativeDirectoryName?
        public RelativeFile(FileInfo file, string relativeDirectoryName)
        {
            Require.NotNull(file, nameof(file));
            Require.NotNull(relativeDirectoryName, nameof(relativeDirectoryName));

            _file = file;
            _relativeDirectoryName = relativeDirectoryName;
        }

        public FileInfo File
        {
            get
            {
                Warrant.NotNull<FileInfo>();

                return _file;
            }
        }

        public string RelativeName
        {
            get
            {
                Warrant.NotNull<string>();

                return Path.Combine(_relativeDirectoryName, _file.Name);
            }
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_file != null);
            Contract.Invariant(_relativeDirectoryName != null);
        }

#endif
    }
}
